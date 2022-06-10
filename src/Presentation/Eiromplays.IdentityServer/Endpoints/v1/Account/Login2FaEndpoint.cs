using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class Login2FaEndpoint : Endpoint<Login2FaRequest, LoginResponse>
{
    private readonly IAuthService _authService;

    public Login2FaEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/account/login2fa");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Login with email and password";
        });
    }
    
    public override async Task HandleAsync(Login2FaRequest req, CancellationToken ct)
    {
        var result = await _authService.Login2FaAsync(req);

        await result.Match(async res => await SendOkAsync(res, ct), exception =>
        {
            if (exception is BadRequestException badRequestException)
            {
                ThrowError(badRequestException.Message);
            }

            return SendErrorsAsync(cancellation: ct);
        });
    }
}