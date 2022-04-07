using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.Export;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/products/export");
        Summary(s =>
        {
            s.Summary = "Export a products.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Export, EIAResource.Products));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        var result = await _mediator.Send(request.ExportProductsRequest, ct);

        await SendStreamAsync(result, "ProductExports.csv", result.Length, contentType: "application/octet-stream",
            cancellation: ct);
    }
}