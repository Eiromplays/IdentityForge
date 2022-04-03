using Eiromplays.IdentityServer.Application.Dashboard;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Dashboard.GetStats;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly ISender _mediator;
    
    public Endpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/dashboard/stats");
        Summary(s =>
        {
            s.Summary = "Get statistics for the dashboard.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Dashboard));
    }

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken cancellationToken)
    {
        Response.Stats = await _mediator.Send(new GetStatsRequest(), cancellationToken);

        return Response;
    }
}