using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.BffUserSessions.DeleteUserSessionByKey;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Delete("/user-sessions/{Key}");
        Summary(s =>
        {
            s.Summary = "Delete a user session.";
        });
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Response.Message = await _userService.DeleteBffUserSessionAsync(req.Key, userId, ct);

        await SendAsync(Response, cancellation: ct);
    }
}