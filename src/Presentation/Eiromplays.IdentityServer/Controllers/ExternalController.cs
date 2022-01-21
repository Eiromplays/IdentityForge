// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using System.Security.Claims;
using System.Text;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;
using Eiromplays.IdentityServer.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.ViewModels.Account;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Eiromplays.IdentityServer.Controllers;

[SecurityHeaders]
[AllowAnonymous]
[ApiVersion("1.0")]
public class ExternalController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly ILogger<ExternalController> _logger;
    private readonly IFluentEmail _fluentEmail;
    private readonly IIdentityService _identityService;
    private readonly ProfilePictureConfiguration _profilePictureConfiguration;

    public ExternalController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<ExternalController> logger,
        IFluentEmail fluentEmail,
        IIdentityService identityService,
        ProfilePictureConfiguration profilePictureConfiguration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _events = events;
        _logger = logger;
        _fluentEmail = fluentEmail;
        _identityService = identityService;
        _profilePictureConfiguration = profilePictureConfiguration;
    }

    /// <summary>
    /// initiate roundtrip to external authentication provider
    /// </summary>
    [HttpGet]
    public IActionResult Challenge(string scheme, string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        // validate returnUrl - either it is a valid OIDC URL or back to a local page
        if (!Url.IsLocalUrl(returnUrl) && !_interaction.IsValidReturnUrl(returnUrl))
        {
            // user might have clicked on a malicious link - should be logged
            throw new Exception("Invalid return URL");
        }

        // start challenge and roundtrip the return URL and scheme
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(scheme, returnUrl);
        properties.RedirectUri = Url.Action(nameof(Callback), new {returnUrl });

        return Challenge(properties, scheme);
    }

    /// <summary>
    /// Post processing of external authentication
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Callback(string returnUrl = "~/")
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            return RedirectToAction("Login", "Account", new {returnUrl });
        }

        // Sign in the user with this external login provider if the user already has a login.
        var singInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
        if (singInResult.Succeeded)
        {
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            // check if external login is in the context of an OIDC request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _events.RaiseAsync(new UserLoginSuccessEvent(info.LoginProvider, info.ProviderKey, user.Id, user.UserName));

            if (context is null) return Redirect(returnUrl);

            return context.IsNativeClient() ? this.LoadingPage("Redirect", returnUrl) : Redirect(returnUrl);
        }
        if (singInResult.RequiresTwoFactor)
        {
            return RedirectToAction("LoginWith2Fa", "Account", new { ReturnUrl = returnUrl });
        }
        if (singInResult.IsLockedOut)
        {
            return RedirectToAction("Lockout", "Account");
        }
        if (singInResult.IsNotAllowed)
        {
            return RedirectToAction("LoginNotAllowed", "Account");
        }

        // If the user does not have an account, then ask the user to create an account.
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["LoginProvider"] = info.LoginProvider;
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var userName = info.Principal.Identity?.Name;

        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email, UserName = userName, DisplayName = userName });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            return View("ExternalLoginFailure");
        }

        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                DisplayName = model.DisplayName,
                Email = model.Email
            };

            if (_profilePictureConfiguration.IsProfilePictureEnabled &&
                _profilePictureConfiguration.AutoGenerateProfilePicture)
            {
                user.ProfilePicture = $"{_profilePictureConfiguration.ProfilePictureGeneratorUrl}/{user.UserName}.svg";
            }

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    if (!await _identityService.CanSignInAsync(user.Id))
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

                        if (sendEmailResponse.Successful)
                            return RedirectToAction("RegisterConfirmation", "Account");

                        ViewData["LoginProvider"] = info.LoginProvider;
                        ViewData["ReturnUrl"] = returnUrl;

                        this.AddErrors(sendEmailResponse.ErrorMessages);
                        return View(model);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return this.RedirectToLocal(returnUrl);
                }
            }

            this.AddErrors(result);
        }

        ViewData["LoginProvider"] = info.LoginProvider;
        ViewData["ReturnUrl"] = returnUrl;

        return View(model);
    }
}