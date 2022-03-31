using Eiromplays.IdentityServer.Application.Dashboard;
using MediatR;
using Shared.Authorization;

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
        Verbs(Http.GET);
        Routes("/dashboard/stats");
        Summary(s =>
        {
            s.Summary = "Get statistics for the dashboard.";
        });
        Version(1);
        Permissions(EIAAction.View, EIAResource.Dashboard);
    }

    public override async Task<Models.Response> HandleAsync(Models.Request request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetStatsRequest(), cancellationToken);
        
        return new Models.Response
        {
            Stats = response
        };
    }
}