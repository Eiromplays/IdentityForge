using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class LogoutEndpoint : Endpoint<LogoutRequest, dynamic>
{
    private readonly IAuthService _authService;

    public LogoutEndpoint(IAuthService authService)
    {
        _authService = authService;
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
        // Logout the user
        Response = await _authService.LogoutAsync<GetLogoutEndpoint>(req, HttpContext);

        await SendOkAsync(Response, ct);
    }
}