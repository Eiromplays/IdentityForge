using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.UpdateProduct;

public class Endpoint : Endpoint<Models.Request, Guid>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/products/{id:guid}");
        Summary(s =>
        {
            s.Summary = "Update a product.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Update, EIAResource.Products));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        if (request.Id != request.Data.Id)
        {
            AddError("Id in request body does not match id in url.");
            await SendErrorsAsync(cancellation: ct);
        }
        
        Response = await _mediator.Send(request.Data, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}