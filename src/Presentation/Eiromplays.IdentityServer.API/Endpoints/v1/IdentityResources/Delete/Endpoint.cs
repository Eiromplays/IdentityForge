using Eiromplays.IdentityServer.Application.Identity.IdentityResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityResources.Delete;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IIdentityResourceService _identityResourceService;

    public Endpoint(IIdentityResourceService identityResourceService)
    {
        _identityResourceService = identityResourceService;
    }

    public override void Configure()
    {
        Delete("/identity-resources/{Id:int}");
        Summary(s =>
        {
            s.Summary = "Delete a IdentityResource.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.ApiScopes));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _identityResourceService.DeleteAsync(req.Id, ct);

        await SendNoContentAsync(cancellation: ct);
    }
}