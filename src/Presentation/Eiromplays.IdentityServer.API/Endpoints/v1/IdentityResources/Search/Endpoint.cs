using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.IdentityResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityResources.Search;

public class Endpoint : Endpoint<Models.Request, PaginationResponse<IdentityResourceDto>>
{
    private readonly IIdentityResourceService _identityResourceService;
    
    public Endpoint(IIdentityResourceService identityResourceService)
    {
        _identityResourceService = identityResourceService;
    }

    public override void Configure()
    {
        Post("/identity-resources/search");
        Summary(s =>
        {
            s.Summary = "Search IdentityResources using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.IdentityResources));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _identityResourceService.SearchAsync(request.Data, ct);

        await SendAsync(Response, cancellation: ct);
    }
}