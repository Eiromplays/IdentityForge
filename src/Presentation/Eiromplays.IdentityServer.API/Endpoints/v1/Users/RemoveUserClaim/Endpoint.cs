using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.RemoveUserClaim;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Delete("/users/{Id}/claims");
        Summary(s =>
        {
            s.Summary = "Remove a claim from a user";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.UserClaims));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.RemoveClaimAsync(req.Id, req.RemoveUserClaimRequest);

        await SendOkAsync(Response, cancellation: ct);
    }
}