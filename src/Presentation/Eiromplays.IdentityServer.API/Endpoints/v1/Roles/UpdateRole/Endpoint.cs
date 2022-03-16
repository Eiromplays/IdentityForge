using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.UpdateRole;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    
    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
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
        var role = await _identityService.FindRoleByIdAsync(req.Id);
        if (role is null)
            ThrowError("Role not found");
        
        role.Name = req.Name;

        var (result, roleId) = await _identityService.UpdateRoleAsync(role);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();
        
        if (string.IsNullOrWhiteSpace(roleId))
            ThrowError("Role was not updated");
        
        await SendCreatedAtAsync("/roles/{id}", roleId, new Models.Response{RoleDto = role}, cancellation: ct);
    }
}