using System.Net;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Roles;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.UpdateRole;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IRoleService _roleService;
    
    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/roles/{id}");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var roleId = await _roleService.CreateOrUpdateAsync(req.RoleDto);

        await SendCreatedAtAsync("/roles/{id}", roleId, new Models.Response{RoleId = roleId}, cancellation: ct);
    }
}