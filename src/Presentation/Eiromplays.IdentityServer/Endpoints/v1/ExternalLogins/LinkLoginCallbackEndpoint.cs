using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

namespace Eiromplays.IdentityServer.Endpoints.v1.ExternalLogins;

public class LinkLoginCallbackEndpoint : EndpointWithoutRequest<LoginResponse>
{
    private readonly IAuthService _authService;

    public LinkLoginCallbackEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Get("/external-logins/link-login/callback");
        Summary(s =>
        {
            s.Summary = "Callback for linking external logins";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await _authService.LinkExternalLoginCallbackAsync(userId, HttpContext);

        if (!string.IsNullOrWhiteSpace(result.ExternalLoginReturnUrl))
        {
            await SendRedirectAsync(result.ExternalLoginReturnUrl, true, ct);
            return;
        }

        await SendOkAsync(result, ct);
    }
}