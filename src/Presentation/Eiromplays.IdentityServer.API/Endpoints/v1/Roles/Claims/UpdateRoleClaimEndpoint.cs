using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Claims;

public class UpdateUserClaimEndpoint : Endpoint<UpdateRoleClaimModels.Request, UpdateRoleClaimModels.Response>
{
    private readonly IRoleService _roleService;

    public UpdateUserClaimEndpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Put("/roles/{Id}/claims/{ClaimId}", "/roles/{Id}/claims");
        Summary(s =>
        {
            s.Summary = "Update role claim";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Update, EiaResource.RoleClaims));
    }

    public override async Task HandleAsync(UpdateRoleClaimModels.Request req, CancellationToken ct)
    {
        Response.Message = await _roleService.UpdateClaimAsync(req.Id, req.ClaimId, req.UpdateRoleClaimRequest);

        await SendOkAsync(Response, cancellation: ct);
    }
}