using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ConfirmEmail;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/confirm-email");
        Summary(s =>
        {
            s.Summary = "Confirm email address for a user.";
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