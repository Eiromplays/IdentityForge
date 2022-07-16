using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ServerSideSessions.Search;

public class Endpoint : Endpoint<ServerSideSessionListFilter, PaginationResponse<ServerSideSessionDto>>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/server-side-sessions/search");
        Summary(s =>
        {
            s.Summary = "Search for server-side sessions";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Search, EiaResource.ServerSideSessions));
    }

    public override async Task HandleAsync(ServerSideSessionListFilter request, CancellationToken ct)
    {
        Response = await _userService.SearchServerSideSessionsAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}