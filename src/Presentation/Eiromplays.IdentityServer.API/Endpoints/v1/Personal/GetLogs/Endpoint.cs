using Eiromplays.IdentityServer.Application.Auditing;
using MediatR;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetLogs;

public class Endpoint : EndpointWithoutRequest<List<AuditDto>>
{
    private readonly ISender _mediatr;

    public Endpoint(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    public override void Configure()
    {
        Get("/personal/logs");
        Summary(s =>
        {
            s.Summary = "Get audit logs of currently logged in user.";
        });
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await _mediatr.Send(new GetMyAuditLogsRequest(), ct);

        await SendAsync(Response, cancellation: ct);
    }
}