using Eiromplays.IdentityServer.Application.Identity.Roles;
using Eiromplays.IdentityServer.Application.Identity.Roles.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Claims;

public class GetRoleClaimsEndpoint : Endpoint<GetRoleClaimsRequest, List<RoleClaimDto>>
{
    private readonly IRoleService _roleService;

    public GetRoleClaimsEndpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Get("/roles/{Id}/claims");
        Summary(s =>
        {
            s.Summary = "Get role claims";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.RoleClaims));
    }

    public override async Task HandleAsync(GetRoleClaimsRequest req, CancellationToken ct)
    {
        Response = await _roleService.GetClaimsAsync(req.Id);

        await SendAsync(Response, cancellation: ct);
    }
}