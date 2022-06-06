using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Org.BouncyCastle.Ocsp;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ICurrentUser _currentUser;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;
    private readonly IEventService _events;
    private readonly LinkGenerator _linkGenerator;

    public AuthService(IIdentityServerInteractionService interaction, ICurrentUser currentUser,
        SignInManager<ApplicationUser> signInManager, IAuthenticationHandlerProvider authenticationHandlerProvider,
        IEventService events, LinkGenerator linkGenerator)
    {
        _interaction = interaction;
        _currentUser = currentUser;
        _signInManager = signInManager;
        _authenticationHandlerProvider = authenticationHandlerProvider;
        _events = events;
        _linkGenerator = linkGenerator;
    }

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
    public virtual SignOutResult SignOut(AuthenticationProperties properties, params string[] authenticationSchemes) =>
        new(authenticationSchemes, properties);
}