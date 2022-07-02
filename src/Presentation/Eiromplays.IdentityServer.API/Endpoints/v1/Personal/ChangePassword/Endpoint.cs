using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.ChangePassword;

public class Endpoint : Endpoint<ChangePasswordRequest, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/personal/change-password");
        Summary(s =>
        {
            s.Summary = "Change password of currently logged in user.";
        });
        Version(1);
        ScopedValidator();
    }

    public override async Task HandleAsync(ChangePasswordRequest req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        // TODO: Return unauthorized if request userId is not the same as the logged in user.
        if (userId != req.UserId)
            req.UserId = userId;

        Response.Message = await _userService.ChangePasswordAsync(req, userId);

        await SendAsync(Response, cancellation: ct);
    }
}