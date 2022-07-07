using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class Send2FaVerificationCodeEndpoint : Endpoint<Send2FaVerificationCodeRequest, Send2FaVerificationCodeResponse>
{
    private readonly IAuthService _authService;

    public Send2FaVerificationCodeEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/account/send-2fa-verification-code");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Send 2FA verification code";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(Send2FaVerificationCodeRequest req, CancellationToken ct)
    {
        var result = await _authService.Send2FaVerificationCodeAsync(req);
        await result.Match(
            async x =>
            {
                await SendOkAsync(x, cancellation: ct);
            },
            async exception =>
            {
                switch (exception)
                {
                    case BadRequestException badRequestException:
                        ThrowError(badRequestException.Message);
                        return;
                    case InternalServerException internalServerException:
                        AddError(internalServerException.Message);
                        await SendErrorsAsync((int)internalServerException.StatusCode, cancellation: ct);
                        return;
                }

                await SendErrorsAsync(cancellation: ct);
            });
    }
}