using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Claims;

public class AddRoleClaimEndpoint : Endpoint<AddRoleClaimModels.Request, AddRoleClaimModels.Response>
{
    private readonly IRoleService _roleService;

    public AddRoleClaimEndpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Post("/roles/{Id}/claims");
        Summary(s =>
        {
            s.Summary = "Add a claim to a role";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Create, EiaResource.RoleClaims));
    }

    public override async Task HandleAsync(AddRoleClaimModels.Request req, CancellationToken ct)
    {
        Response.Message = await _roleService.AddClaimAsync(req.Id, req.AddRoleClaimRequest);

        await SendOkAsync(Response, cancellation: ct);
    }
}