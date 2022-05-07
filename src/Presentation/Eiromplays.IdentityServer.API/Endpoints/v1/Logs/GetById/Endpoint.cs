using Eiromplays.IdentityServer.Application.Auditing;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Logs.GetById;

public class Endpoint : Endpoint<Models.Request, AuditDto>
{
    private readonly IAuditService _auditService;

    public Endpoint(IAuditService auditService)
    {
        _auditService = auditService;
    }

    public override void Configure()
    {
        Get("/logs/{Id}");
        Summary(s =>
        {
            s.Summary = "Get a log.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.AuditLog));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response = await _auditService.GetTrailAsync(req.Id, ct);
        
        await SendOkAsync(Response, cancellation: ct);
    }
}