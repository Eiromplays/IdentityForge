using System.Net;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Configuration;
using Eiromplays.IdentityServer.Contracts.v1.Requests.Account;
using Eiromplays.IdentityServer.Contracts.v1.Responses.Account;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class LogoutEndpoint : Endpoint<LogoutRequest, object>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;
    private readonly IEventService _events;
    
    public LogoutEndpoint(SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService interaction,
        IAuthenticationHandlerProvider authenticationHandlerProvider, IEventService events)
    {
        _signInManager = signInManager;
        _interaction = interaction;
        _authenticationHandlerProvider = authenticationHandlerProvider;
        _events = events;
    }

    public override void Configure()
    {
        Post("/account/logout");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Logout the user";
        });
    }
    
    public override async Task HandleAsync(LogoutRequest req, CancellationToken ct)
    {
        // build a response so the logged out page knows what to display
        var response = await BuildLoggedOutResponseAsync(req.LogoutId);

        if (User.Identity?.IsAuthenticated == true)
        {
            // delete local authentication cookie
            await _signInManager.SignOutAsync();

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        // check if we need to trigger sign-out at an upstream identity provider
        if (!response.TriggerExternalSignout)
        {
            await SendOkAsync(response, ct);
            return;
        }
        // build a return URL so the upstream provider will redirect back
        // to us after the user has logged out. this allows us to then
        // complete our single sign-out processing.
        var url = Url.Action("Logout", new { logoutId = Response.LogoutId });

        // this triggers a redirect to the external provider for sign-out
        await SendOkAsync(SignOut(new AuthenticationProperties { RedirectUri = url }, response.ExternalAuthenticationScheme!), ct);
    }

    public virtual SignOutResult SignOut(AuthenticationProperties properties, params string[] authenticationSchemes) =>
        new(authenticationSchemes, properties);
    
    private async Task<LogoutResponse> BuildLoggedOutResponseAsync(string logoutId)
    {
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        var vm = new LogoutResponse
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