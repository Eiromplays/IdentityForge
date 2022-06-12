using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;
using Microsoft.AspNetCore.Authentication;

namespace Eiromplays.IdentityServer.Endpoints.v1.ExternalLogins;

public class LinkLoginEndpoint : Endpoint<LinkExternalLoginRequest, AuthenticationProperties>
{
    private readonly IAuthService _authService;

    public LinkLoginEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Get("/external-logins/link-login");
        Summary(s =>
        {
            s.Summary = "Link an external login";
        });
        Version(1);
    }

    public override async Task HandleAsync(LinkExternalLoginRequest req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await _authService.LinkExternalLoginAsync<LinkLoginCallbackEndpoint>(req, userId, HttpContext.Response);
    }
}