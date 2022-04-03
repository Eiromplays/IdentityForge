using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.UpdatePermissions;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IRoleService _roleService;
    
    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Put("/roles/{Id}/permissions");
        Summary(s =>
        {
            s.Summary = "Update a role's permissions.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Update, EIAResource.RoleClaims));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _roleService.UpdatePermissionsAsync(req.UpdateRolePermissionsRequest, ct);

        if (req.Id != req.UpdateRolePermissionsRequest.RoleId) 
        {
            AddError("UserId and Id must match.");
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        await SendOkAsync(Response, cancellation: ct);
    }
}