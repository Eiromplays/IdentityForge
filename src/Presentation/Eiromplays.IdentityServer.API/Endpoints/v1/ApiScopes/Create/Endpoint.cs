using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.Create;

public class Endpoint : Endpoint<CreateApiScopeRequest, Models.Response>
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
        Policies(EiaPermission.NameFor(EiaAction.Create, EiaResource.ApiScopes));
        ScopedValidator();
    }

    public override async Task HandleAsync(CreateApiScopeRequest req, CancellationToken ct)
    {
        await SendOkAsync(
            new Models.Response
        {
            Message = await _apiScopeService.CreateAsync(req, ct)
        },
            ct);
    }
}