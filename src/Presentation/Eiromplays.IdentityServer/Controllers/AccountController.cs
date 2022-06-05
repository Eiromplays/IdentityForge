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
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.ViewModels;
using Microsoft.Extensions.Options;
using Shared.Authorization;

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
    private readonly IUserService _userService;
    
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
        UrlEncoder urlEncoder, IUserService userService)
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
        _userService = userService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult IsAuthenticated()
    {
        return Ok(User.Identity?.IsAuthenticated);
    }

    [HttpGet]
    public async Task<IActionResult> TwoFactorAuthentication()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var model = new TwoFactorAuthenticationViewModel
        {
            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) is not null,
            Is2FaEnabled = user.TwoFactorEnabled,
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
        };

        return Ok(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> ForgetTwoFactorClient()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        await _signInManager.ForgetTwoFactorClientAsync();

        return NoContent();
    }
    
    [u8HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
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

        await _userManager.SetTwoFactorEnabledAsync(user, true); var userId = await _userManager.GetUserIdAsync(user);

        if (await _userManager.CountRecoveryCodesAsync(user) != 0)
            return NoContent();
        
        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        
        return Ok(recoveryCodes.ToList());
    }

    [HttpPost]
    public async Task<IActionResult> DisableAuthenticator()
    {
        var userId = User.GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        return Ok(await _userService.DisableTwoFactorAsync(userId));
    }
    
    [HttpPost]
    public async Task<IActionResult> ResetAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);

        return NoContent();
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateRecoveryCodes()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        if (!user.TwoFactorEnabled)
        {
            throw new BadRequestException("Two factor authentication is not enabled");
        }

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        return Ok(recoveryCodes.ToList());
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