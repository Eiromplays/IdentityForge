using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.Delete;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IApiScopeService _apiScopeService;

    public Endpoint(IApiScopeService apiScopeService)
    {
        _apiScopeService = apiScopeService;
    }

    public override void Configure()
    {
        Delete("/api-scopes/{Id:int}");
        Summary(s =>
        {
            s.Summary = "Delete a ApiScope.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.ApiScopes));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _apiScopeService.DeleteAsync(req.Id, ct);

        await SendNoContentAsync(cancellation: ct);
    }
}