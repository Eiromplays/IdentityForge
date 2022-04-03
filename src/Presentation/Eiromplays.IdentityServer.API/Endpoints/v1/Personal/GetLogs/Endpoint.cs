using Eiromplays.IdentityServer.Application.Auditing;
using MediatR;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetLogs;

public class Endpoint : Endpoint<Models.Request, Models.Response>
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

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Response.Logs = await _mediatr.Send(new GetMyAuditLogsRequest(), ct);
        
        await SendAsync(Response, cancellation: ct);
    }
}