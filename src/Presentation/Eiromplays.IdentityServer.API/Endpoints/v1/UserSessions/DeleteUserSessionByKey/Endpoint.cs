using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.UserSessions.DeleteUserSessionByKey;

public class Endpoint : Endpoint<Models.Request, UserSessionDto>
{
    private readonly IUserService _userService;
    
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/user-sessions/{Key}");
        Summary(s =>
        {
            s.Summary = "Get a user session.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response = await _userService.GetUserSessionAsync(req.Key, ct);

        await SendAsync(Response, cancellation: ct);
    }
}