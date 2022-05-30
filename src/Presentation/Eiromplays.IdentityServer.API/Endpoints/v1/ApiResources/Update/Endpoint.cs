using Eiromplays.IdentityServer.Application.Identity.ApiResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiResources.Update;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IApiResourceService _apiResourceService;
    
    public Endpoint(IApiResourceService apiResourceService)
    {
        _apiResourceService = apiResourceService;
    }

    public override void Configure()
    {
        Put("/api-resources/{Id}");
        Summary(s =>
        {
            s.Summary = "Update a ApiResource.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Update, EIAResource.ApiResources));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _apiResourceService.UpdateAsync(req.Data, req.Id, ct);
        
        await SendNoContentAsync(cancellation: ct);
    }
}