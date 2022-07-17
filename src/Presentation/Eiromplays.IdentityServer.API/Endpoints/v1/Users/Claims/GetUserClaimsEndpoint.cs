using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Claims;

public class GetUserClaimsEndpoint : Endpoint<GetUserClaimsRequest, List<UserClaimDto>>
{
    private readonly IUserService _userService;

    public GetUserClaimsEndpoint(IUserService userService)
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

    public override async Task HandleAsync(GetUserClaimsRequest req, CancellationToken ct)
    {
        Response = await _userService.GetClaimsAsync(req.Id);

        await SendAsync(Response, cancellation: ct);
    }
}