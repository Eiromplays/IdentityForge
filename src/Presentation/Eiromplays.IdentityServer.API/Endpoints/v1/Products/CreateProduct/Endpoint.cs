using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.CreateProduct;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/products");
        Summary(s =>
        {
            s.Summary = "Create a new product.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Create, EIAResource.Products));
    }

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.Id = await _mediator.Send(request.CreateProductRequest, ct);

        return Response;
    }
}