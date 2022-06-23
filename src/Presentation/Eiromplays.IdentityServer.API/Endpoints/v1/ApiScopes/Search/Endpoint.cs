using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.Search;

public class Endpoint : Endpoint<ApiScopeListFilter, PaginationResponse<ApiScopeDto>>
{
    private readonly IApiScopeService _apiScopeService;
    
    public Endpoint(IApiScopeService apiScopeService)
    {
        _apiScopeService = apiScopeService;
    }

    public override void Configure()
    {
        Post("/api-scopes/search");
        Summary(s =>
        {
            s.Summary = "Search ApiScopes using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.ApiScopes));
    }

    public override async Task HandleAsync(ApiScopeListFilter request, CancellationToken ct)
    {
        Response = await _apiScopeService.SearchAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}