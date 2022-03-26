using Eiromplays.IdentityServer.Application.Identity.Roles;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.GetRole;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IRoleService _roleService;
    
    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/roles/{Id}");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var role = await _roleService.GetByIdAsync(req.Id);

        await SendAsync(new Models.Response {RoleDto = role}, cancellation: ct);
    }
}