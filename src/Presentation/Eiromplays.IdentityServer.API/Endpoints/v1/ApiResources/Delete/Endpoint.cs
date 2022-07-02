using Eiromplays.IdentityServer.Application.Identity.ApiResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiResources.Delete;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IApiResourceService _apiResourceService;

    public Endpoint(IApiResourceService apiResourceService)
    {
        _apiResourceService = apiResourceService;
    }

    public override void Configure()
    {
        Delete("/api-resources/{Id:int}");
        Summary(s =>
        {
            s.Summary = "Delete a ApiResource.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.ApiResources));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _apiResourceService.DeleteAsync(req.Id, ct);

        await SendNoContentAsync(cancellation: ct);
    }
}