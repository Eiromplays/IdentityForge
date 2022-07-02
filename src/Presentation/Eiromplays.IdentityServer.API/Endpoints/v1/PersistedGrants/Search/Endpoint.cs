using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.Search;

public class Endpoint : Endpoint<PersistedGrantListFilter, PaginationResponse<PersistedGrantDto>>
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
        Policies(EiaPermission.NameFor(EiaAction.Search, EiaResource.PersistedGrants));
    }

    public override async Task HandleAsync(PersistedGrantListFilter request, CancellationToken ct)
    {
        Response = await _persistedGrantService.SearchAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}