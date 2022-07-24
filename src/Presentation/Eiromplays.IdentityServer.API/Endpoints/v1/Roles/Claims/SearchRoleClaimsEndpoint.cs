using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.Roles;
using Eiromplays.IdentityServer.Application.Identity.Roles.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Claims;

public class SearchUserClaimsEndpoint : Endpoint<RoleClaimListFilter, PaginationResponse<RoleClaimDto>>
{
    private readonly IRoleService _roleService;

    public SearchUserClaimsEndpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Post("/roles/{RoleId}/claims-search", "/roles/claims-search");
        Summary(s =>
        {
            s.Summary = "Search for role claims";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Search, EiaResource.RoleClaims));
    }

    public override async Task HandleAsync(RoleClaimListFilter request, CancellationToken ct)
    {
        Response = await _roleService.SearchClaims(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}