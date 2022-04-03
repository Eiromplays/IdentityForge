using Eiromplays.IdentityServer.Application.Catalog.Products;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.GetProduct;

public class Endpoint : Endpoint<Models.Request, Models.Response>
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

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.ProductDetails = await _mediator.Send(new GetProductRequest(request.Id), ct);

        return Response;
    }
}