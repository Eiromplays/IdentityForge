using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Claims;

public class RemoverUserClaimEndpoint : Endpoint<RemoveUserClaimModels.Request, RemoveUserClaimModels.Response>
{
    private readonly IUserService _userService;

    public RemoverUserClaimEndpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Delete("/users/{Id}/claims/{ClaimId}", "/users/{Id}/claims");
        Summary(s =>
        {
            s.Summary = "Remove a claim from a user";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.UserClaims));
    }

    public override async Task HandleAsync(RemoveUserClaimModels.Request req, CancellationToken ct)
    {
        Response.Message = await _userService.RemoveClaimAsync(req.Id, req.ClaimId);

        await SendOkAsync(Response, cancellation: ct);
    }
}