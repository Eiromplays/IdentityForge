using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.CreateBrand;

public class Endpoint : Endpoint<Models.Request, Guid>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/brands");
        Summary(s =>
        {
            s.Summary = "Create a new brand.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Create, EIAResource.Brands));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _mediator.Send(request.Data, ct);

        await SendAsync(Response, cancellation: ct);
    }
}