using Eiromplays.IdentityServer.Application.Catalog.Brands;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.DeleteBrand;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/brands/{id:guid}");
        Summary(s =>
        {
            s.Summary = "Delete a brand.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Delete, EIAResource.Brands));
    }

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.Id = await _mediator.Send(new DeleteBrandRequest(request.Id), ct);

        return Response;
    }
}