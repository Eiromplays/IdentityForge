using Eiromplays.IdentityServer.Application.Catalog.Brands;
using Eiromplays.IdentityServer.Application.Common.Models;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.Search;

public class Endpoint : Endpoint<Models.Request, PaginationResponse<BrandDto>>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/brands/search");
        Summary(s =>
        {
            s.Summary = "Search brands using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.Brands));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _mediator.Send(request.SearchBrandsRequest, ct);

        await SendAsync(Response, cancellation: ct);
    }
}