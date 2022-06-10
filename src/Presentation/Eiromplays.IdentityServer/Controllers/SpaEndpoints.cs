/*using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Configuration;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.ViewModels.Account;
using IdentityModel;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Eiromplays.IdentityServer.Controllers;

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

public class ExternalLoginConfirmationResponse
{
    public string Message { get; set; } = default!;
    public string ReturnUrl { get; set; } = default!;
}

[Route("spa")]
[AllowAnonymous]
public class SpaEndpoints : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IServerUrls _serverUrls;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserResolver<ApplicationUser> _userResolver;
    private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;
    private readonly IEventService _events;
    private readonly IClientStore _clientStore;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;
    private readonly SpaConfiguration _spaConfiguration;

    public SpaEndpoints(IIdentityServerInteractionService interaction, IServerUrls serverUrls,
        SignInManager<ApplicationUser> signInManager, IUserResolver<ApplicationUser> userResolver,
        IAuthenticationHandlerProvider authenticationHandlerProvider, IEventService eventService,
        IClientStore clientStore, IAuthenticationSchemeProvider schemeProvider,
        UserManager<ApplicationUser> userManager, IUserService userService,
        IOptions<SpaConfiguration> spaConfiguration)
    {
        _interaction = interaction;
        _serverUrls = serverUrls;
        _signInManager = signInManager;
        _userResolver = userResolver;
        _authenticationHandlerProvider = authenticationHandlerProvider;
        _events = eventService;
        _clientStore = clientStore;
        _schemeProvider = schemeProvider;
        _userManager = userManager;
        _userService = userService;
        _spaConfiguration = spaConfiguration.Value;
    }

    [HttpGet("error")]
    public async Task<IActionResult> Error(string errorId)
    {
        var errorMessage = await _interaction.GetErrorContextAsync(errorId);
        
        return Ok(errorMessage);
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
    
        
    [HttpGet("login")]
    public async Task<IActionResult> Login(string returnUrl)
    {
        // build a model so we know what to show on the login page
        var vm = await BuildLoginViewModelAsync(returnUrl);
            
        if (vm.EnableLocalLogin == false && vm.ExternalProviders.Count() == 1)
        {
            // only one option for logging in
            return ExternalLogin(vm.ExternalProviders.First().AuthenticationScheme, returnUrl);
        }
        
        return Ok(vm);
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
                response.SignInResult = loginResult;
                
                if (!loginResult.Succeeded)
                {
                    if (loginResult.RequiresTwoFactor)
                    {
                        return RedirectToAction(nameof(LoginWith2Fa), new { rememberMe = model.RememberMe, returnUrl = model.ReturnUrl });
                    }

                    if (loginResult.IsLockedOut)
                    {
                        // TODO: should probably send an email to the user

                        return BadRequest(response);
                    }
                        
                    response.Error = "Invalid username or password";
                    return BadRequest(response);
                }
                
                var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

                if (context is not null)
                {
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    response.ValidReturnUrl = model.ReturnUrl;

                    return Ok(response);
                }

                // request for a local page
                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    response.ValidReturnUrl = _serverUrls.BaseUrl;
                }
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

            // TODO: See if I can improve this
            if (_interaction.IsValidReturnUrl(url))
                response.ValidReturnUrl = url ?? _serverUrls.BaseUrl;

            return Ok(response);
        }

        response.Error = "Invalid authentication code";

        return BadRequest(response);
    }
        
    [HttpGet("ExternalLoginCallback")]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (remoteError != null)
            throw new BadRequestException("Error from external provider", new List<string> { remoteError });
            
        var response = new LoginConsentResponse();
        
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            return Redirect($"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/login");
        }
        
        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true);
        if (result.Succeeded)
        {
            var url = returnUrl != null ? Uri.UnescapeDataString(returnUrl) : null;
            
            return Redirect(url ?? _spaConfiguration.IdentityServerUiBaseUrl);
        }
        
        response.SignInResult = result;
        if (result.RequiresTwoFactor)
        {
            return RedirectToAction(nameof(LoginWith2Fa), new { returnUrl, RememberMe = true });
        }

        if (result.IsLockedOut)
        {
            // TODO: should probably send an email to the user

            return Redirect($"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/lockout");
        }

        if (result.IsNotAllowed)
        {
            return Redirect($"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/not-allowed");
        }

        // If the user does not have an account, then ask the user to create an account.
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var userName = info.Principal.Identity?.Name;
        
        return Redirect(
            $"{_spaConfiguration.IdentityServerUiBaseUrl}/auth/external-login-confirmation/{email}/{userName}/{info.LoginProvider}?returnUrl={returnUrl}");
    }
        
    [HttpPost("ExternalLogin")]
    [HttpGet("ExternalLogin")]
    public IActionResult ExternalLogin(string? provider, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(provider))
            throw new BadRequestException("Provider is required");

        // Request a redirect to the external login provider.
        var redirectUrl = Url.Action("ExternalLoginCallback", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        return Challenge(properties, provider);
    }

    [HttpGet("ExternalLoginConfirmation")]
    public async Task<IActionResult> GetExternalLoginConfirmation(string userId, string code, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
     
        await _userService.ConfirmEmailAsync(userId, code); 
        
        var url = returnUrl is not null ? Uri.UnescapeDataString(returnUrl) : null;
        var context = await _interaction.GetAuthorizationContextAsync(url);

        if (context != null && !string.IsNullOrWhiteSpace(url))
        {
            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            return Redirect(url);
        }

        return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : Redirect(url ?? _serverUrls.BaseUrl);
    }
    
    [HttpPost("ExternalLoginConfirmation")]
    public async Task<IActionResult> ExternalLoginConfirmation([FromBody]CreateExternalUserRequest request, string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return BadRequest(new ExternalLoginConfirmationResponse{Message = "Error loading external login information during confirmation."});
        }
        
        var createUserResponse = await _userService.CreateExternalAsync(request, $"{HttpContext.Request?.Scheme}://{HttpContext.Request?.Host.ToString()}/");

        var addLoginMessage = await _userService.AddLoginAsync(createUserResponse.UserId, info.Adapt<UserLoginInfoDto>());

        // TODO: Check if user should be logged in automatically
        /*if (_signInManager.CanSignInAsync())
        await _signInManager.SignInAsync(user, isPersistent: false);

        var response = new ExternalLoginConfirmationResponse
        {
            Message = string.Join(Environment.NewLine, new List<string> { createUserResponse.Message, addLoginMessage })
        };
        
        var url = returnUrl is not null ? Uri.UnescapeDataString(returnUrl) : null;
        var context = await _interaction.GetAuthorizationContextAsync(url);

        if (context != null && !string.IsNullOrWhiteSpace(url))
        {
            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            response.ReturnUrl = url;

            return Ok(response);
        }

        // request for a local page
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        if (string.IsNullOrEmpty(returnUrl))
        {
            response.ReturnUrl = _serverUrls.BaseUrl;
        }

        return Ok(response);

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

    // HELPER METHODS
    private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;
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
            .Where(x => x.DisplayName != null)
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            }).ToList();

        var allowLocal = true;
        if (context?.Client.ClientId == null)
            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Login = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
            
        var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
        if (client == null)
            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Login = context.LoginHint,
                ExternalProviders = providers.ToArray()
            };
            
        allowLocal = client.EnableLocalLogin;

        if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
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
}*/