using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ServerSideSessions.DeleteServerSideSessionByKey;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Delete("/server-side-sessions/{Key}");
        Summary(s =>
        {
            s.Summary = "Delete a server-side session by key";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.ServerSideSessions));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.DeleteServerSideSessionAsync(req.Key, cancellationToken: ct);

        await SendAsync(Response, cancellation: ct);
    }
}