using Eiromplays.IdentityServer.Application.Catalog.Brands;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.GetBrand;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/brands/{id:guid}");
        Summary(s =>
        {
            s.Summary = "Get brand details.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Brands));
    }

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.Brand = await _mediator.Send(new GetBrandRequest(request.Id), ct);

        return Response;
    }
}