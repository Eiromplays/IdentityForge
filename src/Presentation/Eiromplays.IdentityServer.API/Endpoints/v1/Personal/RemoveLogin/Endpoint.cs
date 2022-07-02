using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Logins;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.RemoveLogin;

public class Endpoint : Endpoint<RemoveLoginRequest, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/personal/remove-external-login");
        Summary(s =>
        {
            s.Summary = "Remove login from currently logged in user.";
        });
        Version(1);
    }

    public override async Task HandleAsync(RemoveLoginRequest req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Response.Message = await _userService.RemoveLoginAsync(req, userId);

        await SendAsync(Response, cancellation: ct);
    }
}