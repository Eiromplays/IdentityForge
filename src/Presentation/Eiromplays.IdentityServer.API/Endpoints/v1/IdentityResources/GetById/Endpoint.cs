using Eiromplays.IdentityServer.Application.Identity.IdentityResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityResources.GetById;

public class Endpoint : Endpoint<Models.Request, IdentityResourceDto>
{
    private readonly IIdentityResourceService _identityResourceService;
    
    public Endpoint(IIdentityResourceService identityResourceService)
    {
        _identityResourceService = identityResourceService;
    }

    public override void Configure()
    {
        Get("/identity-resources/{Id}");
        Summary(s =>
        {
            s.Summary = "Get IdentityResource details.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.ApiScopes));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _identityResourceService.GetAsync(request.Id, ct);

        await SendAsync(Response, cancellation: ct);
    }
}