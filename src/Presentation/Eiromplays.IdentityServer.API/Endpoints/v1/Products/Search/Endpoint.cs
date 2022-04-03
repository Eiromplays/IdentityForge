using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.Search;

public class Endpoint : Endpoint<Models.Request, Models.Response>
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

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.Products = await _mediator.Send(request.SearchProductsRequest, ct);

        return Response;
    }
}