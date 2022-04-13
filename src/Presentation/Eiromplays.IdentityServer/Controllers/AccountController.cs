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
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Controllers;

[SecurityHeaders]
[AllowAnonymous]
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

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IAuthenticationSchemeProvider schemeProvider,
        IEventService events, IFluentEmail fluentEmail,
        IAuthenticationHandlerProvider authenticationHandlerProvider,
        IOptions<AccountConfiguration> accountConfigurationOptions,
        IUserResolver<ApplicationUser> userResolver)
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
    }

    [HttpGet]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult IsAuthenticated()
    {
        return Ok(User.Identity?.IsAuthenticated);
    }
    

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> LoginWith2Fa(bool rememberMe, string? returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user is null)
        {
            throw new InvalidOperationException("Unable to get user");
        }

        var model = new LoginWith2FaViewModel
        {
            ReturnUrl = returnUrl,
            RememberMe = rememberMe
        };

        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginWith2Fa(LoginWith2FaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null)
            throw new InvalidOperationException("Unable to get user");

        var authenticatorCode = model.TwoFactorCode?.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.RememberMachine);

        if (result.Succeeded)
        {
            return LocalRedirect(string.IsNullOrEmpty(model.ReturnUrl) ? "~/" : model.ReturnUrl);
        }

        if (result.IsLockedOut)
        {
            return View("Lockout");
        }

        ModelState.AddModelError(string.Empty, "Invalid authentication code");

        return View(model);
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
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string? userId, string? code)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
        {
            return View("Error");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return View("Error");
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        var result = await _userManager.ConfirmEmailAsync(user, code);
        return View(result.Succeeded ? "ConfirmEmail" : "Error");
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
}