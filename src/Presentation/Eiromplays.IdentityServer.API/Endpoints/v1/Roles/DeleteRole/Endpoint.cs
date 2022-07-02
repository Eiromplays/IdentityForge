using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.DeleteRole;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IRoleService _roleService;

    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Delete("/roles/{Id}");
        Summary(s =>
        {
            s.Summary = "Delete a role.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.Roles));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _roleService.DeleteAsync(req.Id);

        await SendAsync(Response, cancellation: ct);
    }
}