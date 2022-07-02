using Eiromplays.IdentityServer.Application.Identity.ApiResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiResources.GetById;

public class Endpoint : Endpoint<Models.Request, ApiResourceDto>
{
    private readonly IApiResourceService _apiResourceService;

    public Endpoint(IApiResourceService apiResourceService)
    {
        _apiResourceService = apiResourceService;
    }

    public override void Configure()
    {
        Get("/api-resources/{Id}");
        Summary(s =>
        {
            s.Summary = "Get ApiResource details.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.ApiResources));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _apiResourceService.GetAsync(request.Id, ct);

        await SendAsync(Response, cancellation: ct);
    }
}