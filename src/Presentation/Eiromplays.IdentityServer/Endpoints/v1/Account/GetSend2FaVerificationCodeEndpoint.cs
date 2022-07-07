using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class GetSend2FaVerificationCodeEndpoint : EndpointWithoutRequest<List<string>>
{
    private readonly IAuthService _authService;

    public GetSend2FaVerificationCodeEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Get("/account/send-2fa-verification-code");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get a 2FA verification code";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _authService.GetSend2FaVerificationCodeAsync();
        await result.Match(
            async x =>
            {
                await SendOkAsync(x.ToList(), cancellation: ct);
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