using Eiromplays.IdentityServer.Application.Common.Extensions;
using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.GetById;

public class Endpoint : Endpoint<Models.Request, ApiScopeDto>
{
    private readonly IApiScopeService _apiScopeService;

    public Endpoint(IApiScopeService apiScopeService)
    {
        _apiScopeService = apiScopeService;
    }

    public override void Configure()
    {
        Get("/api-scopes/{Id}");
        Summary(s =>
        {
            s.Summary = "Get ApiScope details.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.ApiScopes));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        await this.ResultToResponseAsync(
            await _apiScopeService.GetAsync(request.Id, ct),
            cancellationToken: ct);
    }
}