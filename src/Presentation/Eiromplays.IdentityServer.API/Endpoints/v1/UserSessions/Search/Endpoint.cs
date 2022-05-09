using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.UserSessions.Search;

public class Endpoint : Endpoint<Models.Request, PaginationResponse<UserSessionDto>>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/user-sessions/search");
        Summary(s =>
        {
            s.Summary = "Search user sessions using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.PersistedGrants));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _userService.SearchSessionsAsync(request.Data, ct);

        await SendAsync(Response, cancellation: ct);
    }
}