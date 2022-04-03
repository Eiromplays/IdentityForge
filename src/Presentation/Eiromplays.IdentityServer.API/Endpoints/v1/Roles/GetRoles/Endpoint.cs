using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.GetRoles;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IRoleService _roleService;
    
    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    public override void Configure()
    {
        Get("/roles");
        Summary(s =>
        {
            s.Summary = "Get a list of all roles.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Roles));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Roles = await _roleService.GetListAsync(ct);
        
        await SendAsync(Response, cancellation: ct);
    }
}