using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using LanguageExt.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ICurrentUser _currentUser;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;
    private readonly IEventService _events;
    private readonly LinkGenerator _linkGenerator;
    private readonly IUserResolver<ApplicationUser> _userResolver;
    private readonly IServerUrls _serverUrls;

    public AuthService(IIdentityServerInteractionService interaction, ICurrentUser currentUser,
        SignInManager<ApplicationUser> signInManager, IAuthenticationHandlerProvider authenticationHandlerProvider,
        IEventService events, LinkGenerator linkGenerator, IUserResolver<ApplicationUser> userResolver, IServerUrls serverUrls)
    {
        _interaction = interaction;
        _currentUser = currentUser;
        _signInManager = signInManager;
        _authenticationHandlerProvider = authenticationHandlerProvider;
        _events = events;
        _linkGenerator = linkGenerator;
        _userResolver = userResolver;
        _serverUrls = serverUrls;
    }

    #region Login

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var response = new LoginResponse();
        
        var user = await _userResolver.GetUserAsync(request.Login);
        if (user is null)
        {
            var badRequest = new BadRequestException("Invalid username or password");
            return new Result<LoginResponse>(badRequest);
        }
        
        var loginResult = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: true);
        response.SignInResult = loginResult;

        if (!loginResult.Succeeded)
        {
            if (loginResult.RequiresTwoFactor)
            {
                response.TwoFactorReturnUrl =
                    $"account/login/2fa?rememberMe={request.RememberMe}&returnUrl={request.ReturnUrl}";
                return new Result<LoginResponse>(response);
            }

            if (loginResult.IsLockedOut)
            {
                // TODO: Option to send email to user to notify them of their account being locked 
                return new Result<LoginResponse>(response);
            }

            response.Error = "Invalid username or password";
            return response;
        }

        var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);

        response.ValidReturnUrl = context is not null ? request.ReturnUrl : _serverUrls.BaseUrl;
        return response;
    }

    #endregion
    

    #region Logout
    
    #endregion
    
    public async Task<GetLogoutResponse> BuildLogoutResponseAsync(string logoutId, bool showLogoutPrompt = true)
    {
        var response = new GetLogoutResponse { LogoutId = logoutId, ShowLogoutPrompt = showLogoutPrompt };
            
        if (_currentUser.IsAuthenticated() != true)
        {
            // if the user is not authenticated, then just show logged out page
            response.ShowLogoutPrompt = false;
            return response;
        }

        var context = await _interaction.GetLogoutContextAsync(logoutId);

        // show the logout prompt. this prevents attacks where the user
        // is automatically signed out by another malicious web page.
        if (context?.ShowSignoutPrompt != false)
            return response;

        // it's safe to automatically sign-out
        response.ShowLogoutPrompt = false;
        return response;
    }

    public async Task<dynamic> LogoutAsync(LogoutRequest request, HttpContext httpContext)
    {
        // build a response so the logged out page knows what to display
        var response = await BuildLoggedOutResponseAsync(request.LogoutId, httpContext);

        if (_currentUser.IsAuthenticated())
        {
            // delete local authentication cookie
            await _signInManager.SignOutAsync();

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(_currentUser.GetSubjectId(), _currentUser.GetDisplayName()));
        }

        // check if we need to trigger sign-out at an upstream identity provider
        if (!response.TriggerExternalSignout)
        {
            return response;
        }
        // build a return URL so the upstream provider will redirect back
        // to us after the user has logged out. this allows us to then
        // complete our single sign-out processing.
        var url = _linkGenerator.GetUriByName(httpContext, "Logout", new { logoutId = response.LogoutId });

        // this triggers a redirect to the external provider for sign-out
        return SignOut(new AuthenticationProperties { RedirectUri = url }, response.ExternalAuthenticationScheme!);
    }

    private async Task<LogoutResponse> BuildLoggedOutResponseAsync(string logoutId, HttpContext httpContext, bool automaticRedirectAfterSignOut = true)
    {
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        var vm = new LogoutResponse
        {
            AutomaticRedirectAfterSignOut = automaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl,
            LogoutId = logoutId
        };

        if (_currentUser.IsAuthenticated() != true) return vm;

        var idp = _currentUser.GetIdentityProvider();

        if (idp is null or Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider) return vm;

        var authenticationHandler = await _authenticationHandlerProvider.GetHandlerAsync(httpContext, idp);

        var providerSupportsSignout = authenticationHandler is IAuthenticationSignOutHandler;

        if (!providerSupportsSignout)
            return vm;

        vm.LogoutId ??= await _interaction.CreateLogoutContextAsync();

        vm.ExternalAuthenticationScheme = idp;

        return vm;
    }
    
    // Helper methods
    protected virtual SignOutResult SignOut(AuthenticationProperties properties, params string[] authenticationSchemes) =>
        new(authenticationSchemes, properties);
}