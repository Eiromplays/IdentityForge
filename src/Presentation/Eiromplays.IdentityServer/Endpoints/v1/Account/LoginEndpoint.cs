using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly IAuthService _authService;

    public LoginEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/account/login");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Login with email and password";
        });
    }
    
    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(req);
        await result.Match(async x =>
        {
            if (!string.IsNullOrWhiteSpace(x.TwoFactorReturnUrl))
                await SendRedirectAsync(x.TwoFactorReturnUrl, true, ct);
            
            await SendOkAsync(x, cancellation: ct);
        }, exception =>
        {
            if (exception is BadRequestException badRequestException)
            {
                ThrowError(badRequestException.Message);
            }

            return SendErrorsAsync(cancellation: ct);
        });
    }
}