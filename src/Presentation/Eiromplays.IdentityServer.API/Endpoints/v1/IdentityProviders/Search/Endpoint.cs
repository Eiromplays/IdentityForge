using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityProviders.Search;

public class Endpoint : Endpoint<IdentityProviderListFilter, PaginationResponse<IdentityProviderDto>>
{
    private readonly IIdentityProviderService _identityProviderService;

    public Endpoint(IIdentityProviderService identityProviderService)
    {
        _identityProviderService = identityProviderService;
    }

    public override void Configure()
    {
        Post("/identity-providers/search");
        Summary(s =>
        {
            s.Summary = "Search identity providers using available filters.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Search, EiaResource.IdentityProviders));
    }

    public override async Task HandleAsync(IdentityProviderListFilter request, CancellationToken ct)
    {
        Response = await _identityProviderService.SearchAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}