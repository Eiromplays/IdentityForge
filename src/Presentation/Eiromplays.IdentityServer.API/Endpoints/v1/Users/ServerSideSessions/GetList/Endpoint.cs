using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ServerSideSessions.GetList;

public class Endpoint : EndpointWithoutRequest<List<ServerSideSessionDto>>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/server-side-sessions");
        Summary(s =>
        {
            s.Summary = "Get list of server-side sessions";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.ServerSideSessions));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await _userService.GetAllServerSideSessionsAsync(ct);

        await SendAsync(Response, cancellation: ct);
    }
}