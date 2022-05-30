using Eiromplays.IdentityServer.Application.Identity.IdentityResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityResources.Update;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IIdentityResourceService _identityResourceService;
    
    public Endpoint(IIdentityResourceService identityResourceService)
    {
        _identityResourceService = identityResourceService;
    }

    public override void Configure()
    {
        Put("/identity-resources/{Id}");
        Summary(s =>
        {
            s.Summary = "Update a IdentityResource.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Update, EIAResource.ApiScopes));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _identityResourceService.UpdateAsync(req.Data, req.Id, ct);
        
        await SendNoContentAsync(cancellation: ct);
    }
}