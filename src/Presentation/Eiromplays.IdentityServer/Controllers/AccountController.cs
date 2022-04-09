// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Configuration;
using Eiromplays.IdentityServer.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Validators.Account;
using Eiromplays.IdentityServer.ViewModels.Account;
using FluentEmail.Core;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Users;
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

    /// <summary>
    /// Show logout page
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        // build a model so the logout page knows what to display
        var vm = await BuildLogoutViewModelAsync(logoutId);

        if (vm.ShowLogoutPrompt == false)
        {
            // if the request for logout was properly authenticated from IdentityServer, then
            // we don't need to show the prompt and can just log the user out directly.
            return await Logout(vm);
        }

        return View(vm);
    }


    /// <summary>
    /// Handle logout page postback
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutInputModel model)
    {
        // build a model so the logged out page knows what to display
        var vm = await BuildLoggedOutViewModelAsync(model.LogoutId!);

        if (User.Identity?.IsAuthenticated == true)
        {
            // delete local authentication cookie
            await _signInManager.SignOutAsync();

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        // check if we need to trigger sign-out at an upstream identity provider
        if (!vm.TriggerExternalSignout) return View("LoggedOut", vm);
        // build a return URL so the upstream provider will redirect back
        // to us after the user has logged out. this allows us to then
        // complete our single sign-out processing.
        var url = Url.Action("Logout", new { logoutId = vm.LogoutId });

        // this triggers a redirect to the external provider for sign-out
        return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme!);
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

    /*****************************************/
    /* Helper APIs for the AccountController */
    /*****************************************/
    private async Task<LoginViewModel> BuildLoginViewModelAsync(string? returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP is not null && await _schemeProvider.GetSchemeAsync(context.IdP) is not null)
        {
            var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var vm = new LoginViewModel
            {
                EnableLocalLogin = local,
                ReturnUrl = returnUrl,
                Login = context.LoginHint,
            };

            if (!local)
            {
                vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
            }

            return vm;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName is not null)
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            }).ToList();

        var allowLocal = true;
        if (context?.Client.ClientId is null)
            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Login = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

        if (client is null)
            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Login = context.LoginHint,
                ExternalProviders = providers.ToArray()
            };

        allowLocal = client.EnableLocalLogin;

        if (client.IdentityProviderRestrictions is not null && client.IdentityProviderRestrictions.Any())
        {
            providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
        }

        return new LoginViewModel
        {
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
            ReturnUrl = returnUrl,
            Login = context.LoginHint,
            ExternalProviders = providers.ToArray()
        };
    }

    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
    {
        var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
        vm.Login = model.Login;
        vm.RememberLogin = model.RememberLogin;
        return vm;
    }

    private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
    {
        var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

        if (User.Identity?.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show logged out page
            vm.ShowLogoutPrompt = false;
            return vm;
        }

        var context = await _interaction.GetLogoutContextAsync(logoutId);

        // show the logout prompt. this prevents attacks where the user
        // is automatically signed out by another malicious web page.
        if (context?.ShowSignoutPrompt != false)
            return vm;

        // it's safe to automatically sign-out
        vm.ShowLogoutPrompt = false;
        return vm;
    }

    private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
    {
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        var vm = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl,
            LogoutId = logoutId
        };

        if (User.Identity?.IsAuthenticated != true) return vm;

        var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

        if (idp is null or Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider) return vm;

        var authenticationHandler = await _authenticationHandlerProvider.GetHandlerAsync(HttpContext, idp);

        var providerSupportsSignout = authenticationHandler is IAuthenticationSignOutHandler;

        if (!providerSupportsSignout)
            return vm;

        vm.LogoutId ??= await _interaction.CreateLogoutContextAsync();

        vm.ExternalAuthenticationScheme = idp;

        return vm;
    }
}