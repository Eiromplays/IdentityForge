using Eiromplays.IdentityServer.Application.Catalog.Products;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.GetDapper;

public class Endpoint : Endpoint<Models.Request, ProductDto>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/products/dapper");
        Summary(s =>
        {
            s.Summary = "Get product details via dapper.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Products));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _mediator.Send(new GetProductViaDapperRequest(request.Id), ct);

        await SendAsync(Response, cancellation: ct);
    }
}