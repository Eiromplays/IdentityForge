using Eiromplays.IdentityServer.Application.Catalog.Brands;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.DeleteRandom;

public class Endpoint : EndpointWithoutRequest<Models.Response>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/brands/delete-random");
        Summary(s =>
        {
            s.Summary = "Delete the brands generated with the generate-random call.Delete the brands generated with the generate-random call.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Clean, EIAResource.Brands));
    }

    public override async Task<Models.Response> HandleAsync(CancellationToken ct)
    {
        Response.Message = await _mediator.Send(new DeleteRandomBrandRequest(), ct);

        return Response;
    }
}