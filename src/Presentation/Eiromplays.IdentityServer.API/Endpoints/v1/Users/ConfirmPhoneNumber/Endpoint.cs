using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ConfirmPhoneNumber;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/confirm-phone-number");
        Summary(s =>
        {
            s.Summary = "Confirm phone number for a user.";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.ConfirmEmailAsync(req.UserId, req.Code, ct);

        await SendAsync(Response, cancellation: ct);
    }
}