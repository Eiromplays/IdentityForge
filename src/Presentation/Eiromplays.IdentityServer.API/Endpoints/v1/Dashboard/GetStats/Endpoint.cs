using Eiromplays.IdentityServer.Application.Dashboard;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Dashboard.GetStats;

public class Endpoint : EndpointWithoutRequest<StatsDto>
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
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.Dashboard));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Response = await _mediator.Send(new GetStatsRequest(), cancellationToken);

        await SendAsync(Response, cancellation: cancellationToken);
    }
}