using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.ApiResources;
using Eiromplays.IdentityServer.Application.Identity.ApiResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiResources.Search;

public class Endpoint : Endpoint<Models.Request, PaginationResponse<ApiResourceDto>>
{
    private readonly IApiResourceService _apiResourceService;
    
    public Endpoint(IApiResourceService apiResourceService)
    {
        _apiResourceService = apiResourceService;
    }

    public override void Configure()
    {
        Post("/api-resources/search");
        Summary(s =>
        {
            s.Summary = "Search ApiResources using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.ApiResources));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _apiResourceService.SearchAsync(request.Data, ct);

        await SendAsync(Response, cancellation: ct);
    }
}