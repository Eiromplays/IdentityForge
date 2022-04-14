// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Validators.Account;
using Eiromplays.IdentityServer.ViewModels.Account;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.ViewModels;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Controllers;

[SecurityHeaders]
[Microsoft.AspNetCore.Authorization.Authorize]
[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clientStore;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IEventService _events;
    private readonly IFluentEmail _fluentEmail;
    private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;
    private readonly AccountConfiguration _accountConfiguration;
    private readonly IUserResolver<ApplicationUser> _userResolver;
    private readonly UrlEncoder _urlEncoder;
    
    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IAuthenticationSchemeProvider schemeProvider,
        IEventService events, IFluentEmail fluentEmail,
        IAuthenticationHandlerProvider authenticationHandlerProvider,
        IOptions<AccountConfiguration> accountConfigurationOptions,
        IUserResolver<ApplicationUser> userResolver,
        UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _clientStore = clientStore;
        _schemeProvider = schemeProvider;
        _events = events;
        _fluentEmail = fluentEmail;
        _authenticationHandlerProvider = authenticationHandlerProvider;
        _accountConfiguration = accountConfigurationOptions.Value;
        _userResolver = userResolver;
        _urlEncoder = urlEncoder;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult IsAuthenticated()
    {
        return Ok(User.Identity?.IsAuthenticated);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        if (_accountConfiguration.RegisterConfiguration is {Enabled: false}) return View("RegisterFailure");

        returnUrl ??= Url.Content("~/");

        ViewData["ReturnUrl"] = returnUrl;
        var validator = new RegisterValidator(_accountConfiguration);

        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid) return View(model);

        var user = new ApplicationUser
        {
            UserName = model.UserName ?? model.Email,
            DisplayName = model.DisplayName ?? model.Email,
            Email = model.Email
        };

        if (_accountConfiguration.ProfilePictureConfiguration is { Enabled: true, AutoGenerate: true })
            user.ProfilePicture = $"{_accountConfiguration.ProfilePictureConfiguration.DefaultUrl}/{user.UserName}.svg";

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            if (await _userManager.IsEmailConfirmedAsync(user))
                return RedirectToAction("RegisterConfirmation", "Account");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var verificationUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code },
                HttpContext.Request.Scheme);

            var confirmEmailViewModel = new ConfirmEmailViewModel
                { Username = user.UserName, VerificationUrl = verificationUrl };

            var sendEmailResponse = await _fluentEmail
                .To(user.Email)
                .Subject("Email Confirmation")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Views/Shared/Templates/Email/ConfirmEmail.cshtml", confirmEmailViewModel)
                .SendAsync();

            if (_signInManager.Options.SignIn.RequireConfirmedAccount && sendEmailResponse.Successful)
            {
                return View("RegisterConfirmation");
            }

            if (!sendEmailResponse.Successful)
            {
                this.AddErrors(sendEmailResponse.ErrorMessages);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        this.AddErrors(result);

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var model = new EnableAuthenticatorViewModel();
        await LoadSharedKeyAndQrCodeUriAsync(user, model);

        return Ok(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> EnableAuthenticator([FromBody] EnableAuthenticatorViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.Where(e => e.Errors.Count > 0)
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                
            throw new BadRequestException("", errors);
        }

        var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var is2FaTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
            user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        if (!is2FaTokenValid)
        {
            await LoadSharedKeyAndQrCodeUriAsync(user, model);
            throw new InternalServerException("Invalid verification code");
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);
        var userId = await _userManager.GetUserIdAsync(user);

        if (await _userManager.CountRecoveryCodesAsync(user) != 0)
            return Ok(userId);
        
        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        //TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

        //return RedirectToAction(nameof(ShowRecoveryCodes));
        return Ok(recoveryCodes.ToList());

    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResendEmailConfirmation(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationViewModel model, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Identifier) ??
                   await _userManager.FindByIdAsync(model.Identifier) ??
                   await _userManager.FindByNameAsync(model.Identifier);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Unable to send verification email, user was not found.");
            return View(model);
        }

        if (await _userManager.IsEmailConfirmedAsync(user))
        {
            ModelState.AddModelError(string.Empty, "Unable to send verification email, email already verified.");
            return View(model);
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var verificationUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code },
            HttpContext.Request.Scheme);

        var confirmEmailViewModel = new ConfirmEmailViewModel
            { Username = user.UserName, VerificationUrl = verificationUrl };

        var sendEmailResponse = await _fluentEmail
            .To(user.Email)
            .Subject("Email Confirmation")
            .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Views/Shared/Templates/Email/ConfirmEmail.cshtml", confirmEmailViewModel)
            .SendAsync();

        if (sendEmailResponse.Successful)
            return View("RegisterConfirmation");

        this.AddErrors(sendEmailResponse.ErrorMessages);
        return View(model);

    }

    [HttpGet]
    public IActionResult LoginNotAllowed()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Lockout()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    public IActionResult RegisterConfirmation()
    {
        return View();
    }
    
    private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
    {
        var sharedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(sharedKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            sharedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        model.SharedKey = sharedKey;
        if (user.Email != null) 
            model.AuthenticatorUri = GenerateQrCodeUri(user.Email, sharedKey);
    }
    
    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
        return string.Format(
            AuthenticatorUriFormat,
            _urlEncoder.Encode("Eiromplays.IdentityServer.Admin"),
            _urlEncoder.Encode(email),
            unformattedKey);
    }
}