// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eiromplays.IdentityServer.Quickstart.Account
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly ILogger<ExternalController> _logger;
        private readonly IdentityOptions _identityOptions;

        public ExternalController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            ILogger<ExternalController> logger,
            IdentityOptions identityOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
            _logger = logger;
            _identityOptions = identityOptions;
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
            properties.RedirectUri = Url.Action(nameof(Callback), new { returnUrl = returnUrl });

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
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var singInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
            if (singInResult.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                // check if external login is in the context of an OIDC request
                var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

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

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        if (!await _signInManager.CanSignInAsync(user))
                        {
                            return View("RegisterConfirmation");
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

        // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
        // this will be different for WS-Fed, SAML2p or other protocols
        private static void ProcessLoginCallback(AuthenticateResult externalResult, ICollection<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal?.Claims.FirstOrDefault(x => x.Type is JwtClaimTypes.SessionId);
            if (sid is not null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var idToken = externalResult.Properties?.GetTokenValue("id_token");
            if (idToken is not null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
            }
        }
    }
}