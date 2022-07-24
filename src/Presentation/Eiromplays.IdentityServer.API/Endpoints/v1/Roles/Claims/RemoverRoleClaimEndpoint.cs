using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Claims;

public class RemoverUserClaimEndpoint : Endpoint<RemoveRoleClaimModels.Request, RemoveRoleClaimModels.Response>
{
    private readonly IRoleService _roleService;

    public RemoverUserClaimEndpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Delete("/roles/{Id}/claims/{ClaimId}", "/roles/{Id}/claims");
        Summary(s =>
        {
            s.Summary = "Remove a claim from a role";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.RoleClaims));
    }

    public override async Task HandleAsync(RemoveRoleClaimModels.Request req, CancellationToken ct)
    {
        Response.Message = await _roleService.RemoveClaimAsync(req.Id, req.ClaimId);

        await SendOkAsync(Response, cancellation: ct);
    }
}