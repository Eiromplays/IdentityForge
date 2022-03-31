using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.GetRoleByIdWithPermissions;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IRoleService _roleService;
    
    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Get("/roles/{Id}/permissions");
        Summary(s =>
        {
            s.Summary = "Get role details with its permissions.";
        });
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.RoleDto = await _roleService.GetByIdWithPermissionsAsync(req.Id, ct);

        await SendAsync(Response, cancellation: ct);
    }
}