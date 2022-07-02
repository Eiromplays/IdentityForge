using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.Update;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IApiScopeService _apiScopeService;

    public Endpoint(IApiScopeService apiScopeService)
    {
        _apiScopeService = apiScopeService;
    }

    public override void Configure()
    {
        Put("/api-scopes/{Id}");
        Summary(s =>
        {
            s.Summary = "Update a ApiScope.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Update, EiaResource.ApiScopes));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _apiScopeService.UpdateAsync(req.Data, req.Id, ct);

        await SendNoContentAsync(cancellation: ct);
    }
}