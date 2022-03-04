using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.CreateRole;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    
    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/roles");
        Version(1);
        Policies("RequireAdministrator");
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (req.RoleDto is null) 
            ThrowError("RoleDto is null");
        
        var (result, roleId) = await _identityService.CreateRoleAsync(req.RoleDto);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();
        
        if (roleId is null)
            ThrowError("Role was not created");
        
        await SendCreatedAtAsync("/roles/{id}", roleId, new Models.Response(){RoleDto = req.RoleDto}, ct);
    }
}