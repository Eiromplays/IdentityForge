using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class GetValidTwoFactorProvidersEndpoint : EndpointWithoutRequest<List<string>>
{
    private readonly IAuthService _authService;

    public GetValidTwoFactorProvidersEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Get("/account/valid-two-factor-providers");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get valid two factor providers for user";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await this.ResultToResponseAsync(
            await _authService.GetValidTwoFactorProvidersAsync(),
            cancellationToken: ct);
    }
}