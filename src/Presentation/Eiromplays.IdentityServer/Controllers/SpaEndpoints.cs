using System.ComponentModel.DataAnnotations;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Configuration;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.ViewModels.Account;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Eiromplays.IdentityServer.Controllers
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(100)]
        public string? Login { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
        [MaxLength(2000)]
        public string? ReturnUrl { get; set; }
    }
    
    public class ConsentRequest
    {
        public bool Deny { get; set; }
        public bool Remember { get; set; }
        [MaxLength(2000)]
        public string? ReturnUrl { get; set; }
    }

    public class LoginConsentResponse
    {
        public string? Error { get; set; }
        
        public SignInResult? SignInResult { get; set; }
        
        public string? ValidReturnUrl { get; set; }
    }

    [Route("spa")]
    [AllowAnonymous]
    public class SpaEndpoints : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IServerUrls _serverUrls;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserResolver<ApplicationUser> _userResolver;
        private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;
        private readonly IEventService _events;

        public SpaEndpoints(IIdentityServerInteractionService interaction, IServerUrls serverUrls,
            SignInManager<ApplicationUser> signInManager, IUserResolver<ApplicationUser> userResolver,
            IAuthenticationHandlerProvider authenticationHandlerProvider, IEventService eventService)
        {
            _interaction = interaction;
            _serverUrls = serverUrls;
            _signInManager = signInManager;
            _userResolver = userResolver;
            _authenticationHandlerProvider = authenticationHandlerProvider;
            _events = eventService;
        }

        [HttpGet("context")]
        public async Task<IActionResult> Context(string returnUrl)
        {
            var authzContext = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (authzContext != null)
            {
                return Ok(new 
                {
                    loginHint = authzContext.LoginHint,
                    idp = authzContext.IdP,
                    tenant = authzContext.Tenant,
                    scopes = authzContext.ValidatedResources.RawScopeValues,
                    client = authzContext.Client.ClientName ?? authzContext.Client.ClientId
                });
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var response = new LoginConsentResponse();

            if (ModelState.IsValid)
            {
                var user = await _userResolver.GetUserAsync(model.Login);

                if (user is not null)
                {
                    var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
                    
                    if (!loginResult.Succeeded)
                    {
                        response.SignInResult = loginResult;
                        if (loginResult.RequiresTwoFactor)
                        {
                            return RedirectToAction(nameof(LoginWith2Fa), new { model.ReturnUrl, RememberMe = model.RememberMe });
                        }

                        if (loginResult.IsLockedOut)
                        {
                            // TODO: should probably send an email to the user

                            return BadRequest(response);
                        }
                        
                        response.Error = "Invalid username or password";
                        return new BadRequestObjectResult(response);
                    }

                    var url = model.ReturnUrl != null ? Uri.UnescapeDataString(model.ReturnUrl) : null;

                    var authzContext = await _interaction.GetAuthorizationContextAsync(url);
                    response.ValidReturnUrl = authzContext != null ? url : _serverUrls.BaseUrl;

                    return Ok(response);
                }
            }

            response.Error = "invalid username or password";
            return new BadRequestObjectResult(response);
        }
        
        [HttpGet("loginWith2fa")]
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

            return Ok(model);
        }

        [HttpPost("loginWith2fa")]
        public async Task<IActionResult> LoginWith2Fa([FromBody] LoginWith2FaViewModel model)
        {
            var response = new LoginConsentResponse();
            
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.Where(e => e.Errors.Count > 0)
                    .SelectMany(e => e.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                throw new BadRequestException("", errors);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user is null)
                throw new InvalidOperationException("Unable to get user");
            
            var authenticatorCode = model.TwoFactorCode?.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result =
                await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                var url = model.ReturnUrl != null ? Uri.UnescapeDataString(model.ReturnUrl) : null;

                var authzContext = await _interaction.GetAuthorizationContextAsync(url);
                response.ValidReturnUrl = authzContext != null ? url : _serverUrls.BaseUrl;

                return Ok(response);
            }

            response.Error = "Invalid authentication code";

            return BadRequest(response);
        }

        [HttpPost("consent")]
        public async Task<IActionResult> Consent([FromBody] ConsentRequest model)
        {
            var response = new LoginConsentResponse();

            if (ModelState.IsValid)
            {
                if (model.ReturnUrl != null)
                {
                    var url = Uri.UnescapeDataString(model.ReturnUrl);

                    var authzContext = await _interaction.GetAuthorizationContextAsync(url);
                    if (authzContext != null)
                    {
                        response.ValidReturnUrl = url;

                        if (model.Deny)
                        {
                            await _interaction.DenyAuthorizationAsync(authzContext, AuthorizationError.AccessDenied);
                        }
                        else
                        {
                            await _interaction.GrantConsentAsync(authzContext,
                                new ConsentResponse
                                {
                                    RememberConsent = model.Remember,
                                    ScopesValuesConsented = authzContext.ValidatedResources.RawScopeValues
                                });
                        }
                    
                        return Ok(response);
                    }
                }
            }

            response.Error = "error";
            return new BadRequestObjectResult(response);
        }

        [HttpGet("error")]
        public async Task<IActionResult> Error(string errorId)
        {
            var errorInfo = await _interaction.GetErrorContextAsync(errorId);
            return Ok(new { 
                errorInfo.Error,
                errorInfo.ErrorDescription
            });
        }

        [HttpGet("logout")]
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
            
            return Ok(new LogoutResponse(vm));
        }

        [HttpPost("logout")]
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
            if (!vm.TriggerExternalSignout) return Ok(new LogoutResponse(null, vm));
            // build a return URL so the upstream provider will redirect back
            // to us after the user has logged out. this allows us to then
            // complete our single sign-out processing.
            var url = Url.Action("Logout", new { logoutId = vm.LogoutId });

            // this triggers a redirect to the external provider for sign-out
            return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme!);
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
}
