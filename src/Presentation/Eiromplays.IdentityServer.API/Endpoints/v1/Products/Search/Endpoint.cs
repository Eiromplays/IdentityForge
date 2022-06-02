using Eiromplays.IdentityServer.Application.Catalog.Products;
using Eiromplays.IdentityServer.Application.Common.Models;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.Search;

public class Endpoint : Endpoint<Models.Request, PaginationResponse<ProductDto>>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/products/search");
        Summary(s =>
        {
            s.Summary = "Search products using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.Products));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _mediator.Send(request.Data, ct);

        await SendAsync(Response, cancellation: ct);
    }
}