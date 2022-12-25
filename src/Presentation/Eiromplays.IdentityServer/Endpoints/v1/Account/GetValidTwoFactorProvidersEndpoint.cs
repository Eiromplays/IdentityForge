using Eiromplays.IdentityServer.Application.Identity.Auth;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class GetValidTwoFactorProvidersEndpoint : EndpointWithoutRequest<IList<string>>
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
        await SendOkAsync(
            await _authService.GetValidTwoFactorProvidersAsync(),
            ct);
    }
}