using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class SendLoginVerificationCodeEndpoint : Endpoint<SendSmsLoginCodeRequest, SendSmsLoginCodeResponse>
{
    private readonly IAuthService _authService;

    public SendLoginVerificationCodeEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/account/send-verification-code");
        Summary(s =>
        {
            s.Summary = "Send verification code to phone, to login with phone";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(SendSmsLoginCodeRequest req, CancellationToken ct)
    {
        var result = await _authService.SendLoginVerificationCodeAsync(req);

        await result.Match(
            async x =>
            {
                await SendOkAsync(x, cancellation: ct);
            },
            exception =>
            {
                if (exception is BadRequestException badRequestException)
                {
                    ThrowError(badRequestException.Message);
                }

                return SendErrorsAsync(cancellation: ct);
            });
    }
}