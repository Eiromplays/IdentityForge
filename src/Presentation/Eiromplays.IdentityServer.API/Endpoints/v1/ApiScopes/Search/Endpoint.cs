using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.Search;

public class Endpoint : Endpoint<Models.Request, PaginationResponse<ApiScopeDto>>
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

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _apiScopeService.SearchAsync(request.Data, ct);

        await SendAsync(Response, cancellation: ct);
    }
}