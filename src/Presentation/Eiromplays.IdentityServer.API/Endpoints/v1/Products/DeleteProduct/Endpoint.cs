using Eiromplays.IdentityServer.Application.Catalog.Products;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.DeleteProduct;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/products/{id:guid}");
        Summary(s =>
        {
            s.Summary = "Delete a product.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Delete, EIAResource.Products));
    }

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.Id = await _mediator.Send(new DeleteProductRequest(request.Id), ct);

        return Response;
    }
}