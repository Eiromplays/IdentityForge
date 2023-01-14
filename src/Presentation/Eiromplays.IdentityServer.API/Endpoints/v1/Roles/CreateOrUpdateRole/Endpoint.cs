using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.CreateOrUpdateRole;

public class Endpoint : Endpoint<CreateOrUpdateRoleRequest, Models.Response>
{
    private readonly IRoleService _roleService;

    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Post("/roles");
        Summary(s =>
        {
            s.Summary = "Create or update a role.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Create, EiaResource.Roles));
    }

    public override async Task HandleAsync(CreateOrUpdateRoleRequest req, CancellationToken ct)
    {
        Response.Message = await _roleService.CreateOrUpdateAsync(req);

        await SendCreatedAtAsync<GetRoleById.Endpoint>(new { req.Id }, Response, generateAbsoluteUrl: true, cancellation: ct);
    }
}