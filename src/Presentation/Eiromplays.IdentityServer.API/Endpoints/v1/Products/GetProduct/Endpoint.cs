using Eiromplays.IdentityServer.Application.Catalog.Products;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.GetProduct;

public class Endpoint : Endpoint<Models.Request, ProductDetailsDto>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/products/{id:guid}");
        Summary(s =>
        {
            s.Summary = "Get product details.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Products));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _mediator.Send(new GetProductRequest(request.Id), ct);

        await SendAsync(Response, cancellation: ct);
    }
}