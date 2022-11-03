using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Domain.Constants;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class GetLogoutEndpoint : Endpoint<GetLogoutRequest, GetLogoutResponse>
{
    private readonly IAuthService _authService;

    public GetLogoutEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Get("/account/logout");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get logout information";
        });
    }

    public override async Task HandleAsync(GetLogoutRequest req, CancellationToken ct)
    {
        await this.ResultToResponseAsync(
            await _authService.BuildLogoutResponseAsync(req.LogoutId, AccountOptions.ShowLogoutPrompt),
            customFunc: async response =>
            {
                if (!response.ShowLogoutPrompt) return false;

                await this.ResultToResponseAsync(
                    await _authService.LogoutAsync<GetLogoutEndpoint>(new LogoutRequest { LogoutId = response.LogoutId }, HttpContext),
                    ct: ct);
                return true;
            },
            ct);
    }
}