using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ResendEmailConfirmationAsync : Endpoint<ResendEmailVerificationRequest, ResendEmailVerificationResponse>
{
    private readonly IUserService _userService;

    public ResendEmailConfirmationAsync(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/account/resend-email-confirmation");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Resend email confirmation";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(ResendEmailVerificationRequest req, CancellationToken ct)
    {
        Response = await _userService.ResendEmailVerificationAsync(req, BaseURL);

        await SendOkAsync(Response, cancellation: ct);
    }
}