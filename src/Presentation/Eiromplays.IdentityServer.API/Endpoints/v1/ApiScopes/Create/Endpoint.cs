using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.Create;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IApiScopeService _apiScopeService;
    
    public Endpoint(IApiScopeService apiScopeService)
    {
        _apiScopeService = apiScopeService;
    }

    public override void Configure()
    {
        Post("/api-scopes");
        Summary(s =>
        {
            s.Summary = "Creates a new ApiScopes.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Create, EIAResource.ApiScopes));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _apiScopeService.CreateAsync(req.Data, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}