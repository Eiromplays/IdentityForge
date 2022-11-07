using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Login;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ResendPhoneNumberConfirmationAsync : Endpoint<ResendPhoneNumberVerificationRequest, ResendPhoneNumberVerificationResponse>
{
    private readonly IUserService _userService;

    public ResendPhoneNumberConfirmationAsync(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/account/resend-phone-number-confirmation");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Resend phone number confirmation";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(ResendPhoneNumberVerificationRequest req, CancellationToken ct)
    {
        Response = await _userService.ResendPhoneNumberVerificationAsync(req, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}