using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ConfirmPhoneNumberEndpoint : Endpoint<ConfirmPhoneNumberRequest, ConfirmPhoneNumberResponse>
{
    private readonly IUserService _userService;

    public ConfirmPhoneNumberEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/account/confirm-phone-number");
        Summary(s =>
        {
            s.Summary = "Confirm phone number for a user.";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ConfirmPhoneNumberRequest req, CancellationToken ct)
    {
        Response.Message = await _userService.ConfirmPhoneNumberAsync(req.UserId, req.Code);

        await SendAsync(Response, cancellation: ct);
    }
}