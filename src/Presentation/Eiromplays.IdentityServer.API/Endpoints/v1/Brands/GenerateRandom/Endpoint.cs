using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.GenerateRandom;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/brands/generate-random");
        Summary(s =>
        {
            s.Summary = "Generate a number of random brands.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Generate, EIAResource.Brands));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response.Message = await _mediator.Send(request.Data, ct);

        await SendAsync(Response, cancellation: ct);
    }
}