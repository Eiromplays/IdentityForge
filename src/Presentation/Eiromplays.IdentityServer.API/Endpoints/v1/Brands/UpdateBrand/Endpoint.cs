using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.UpdateBrand;

public class Endpoint : Endpoint<Models.Request, Guid>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/brands/{id:guid}");
        Summary(s =>
        {
            s.Summary = "Update a brand.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Update, EIAResource.Brands));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        if (request.Id != request.UpdateBrandRequest.Id)
        {
            AddError("Id in request body does not match id in url.");
            await SendErrorsAsync(cancellation: ct);
        }
        
        Response = await _mediator.Send(request.UpdateBrandRequest, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}