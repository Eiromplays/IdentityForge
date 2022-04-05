using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.Search;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IPersistedGrantService _persistedGrantService;
    
    public Endpoint(IPersistedGrantService persistedGrantService)
    {
        _persistedGrantService = persistedGrantService;
    }

    public override void Configure()
    {
        Post("/persisted-grants/search");
        Summary(s =>
        {
            s.Summary = "Search persisted grants using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.PersistedGrants));
    }

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.PersistedGrants = await _persistedGrantService.SearchAsync(request.PersistedGrantListFilter, ct);

        return Response;
    }
}