using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.BffUserSessions.GetList;

public class Endpoint : EndpointWithoutRequest<List<UserSessionDto>>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/user-sessions");
        Summary(s =>
        {
            s.Summary = "Get a list of all user sessions.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.Users));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await _userService.GetAllBffUserSessions(ct);

        await SendAsync(Response, cancellation: ct);
    }
}