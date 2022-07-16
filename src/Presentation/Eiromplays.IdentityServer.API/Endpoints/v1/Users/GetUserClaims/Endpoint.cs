using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUserClaims;

public class Endpoint : Endpoint<Models.Request, List<UserClaimDto>>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/{Id}/claims");
        Summary(s =>
        {
            s.Summary = "Get user claims";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.UserClaims));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response = await _userService.GetClaimsAsync(req.Id);

        await SendAsync(Response, cancellation: ct);
    }
}