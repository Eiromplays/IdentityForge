using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ResetPassword;

public class Endpoint : Endpoint<ResetPasswordRequest, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/reset-password");
        Summary(s =>
        {
            s.Summary = "Reset a user's password.";
        });
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        Response.Message = await _userService.ResetPasswordAsync(req);

        await SendAsync(Response, cancellation: ct);
    }
}