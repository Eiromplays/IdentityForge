using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.CreateOrUpdateRole;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IRoleService _roleService;
    
    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Post("/roles");
        Summary(s =>
        {
            s.Summary = "Create or update a role.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Create, EIAResource.Roles));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.RoleId = await _roleService.CreateOrUpdateAsync(req.Data);

        await SendCreatedAtAsync<GetRoleById.Endpoint>(Response.RoleId, Response, cancellation: ct);
    }
}