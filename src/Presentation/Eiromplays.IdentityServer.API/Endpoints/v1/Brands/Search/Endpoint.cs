using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.Search;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/brands/search");
        Summary(s =>
        {
            s.Summary = "Search brands using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.Brands));
    }

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.Brands = await _mediator.Send(request.SearchBrandsRequest, ct);

        return Response;
    }
}