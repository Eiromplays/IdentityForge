using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.Endpoints.v1.Account;

public class ResetPasswordEndpoint : Endpoint<ResetPasswordRequest, ResetPasswordResponse>
{
    private readonly IUserService _userService;

    public ResetPasswordEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/account/reset-password");
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