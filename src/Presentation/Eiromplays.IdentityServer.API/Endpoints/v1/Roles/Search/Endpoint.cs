using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Search;

public class Endpoint : Endpoint<RoleListFilter, PaginationResponse<RoleDto>>
{
    private readonly IRoleService _roleService;
    
    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    public override void Configure()
    {
        Post("/roles/search");
        Summary(s =>
        {
            s.Summary = "Search roles using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.Roles));
    }
    
    public override async Task HandleAsync(RoleListFilter request, CancellationToken ct)
    {
        Response = await _roleService.SearchAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}