using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ExternalLoginCallbackEndpoint : Endpoint<ExternalLoginCallbackRequest, LoginResponse>
{
    private readonly IAuthService _authService;

    public ExternalLoginCallbackEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Get("/account/external-logins/callback");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Callback for external login, during the sign-in process.";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(ExternalLoginCallbackRequest req, CancellationToken ct)
    {
        var result = await _authService.ExternalLoginCallbackAsync(req, BaseURL);
        if (!string.IsNullOrWhiteSpace(result.ExternalLoginReturnUrl))
        {
            await SendRedirectAsync(result.ExternalLoginReturnUrl, true, ct);
            return;
        }

        await SendOkAsync(result, ct);
    }
}