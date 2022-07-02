using Eiromplays.IdentityServer.Application.Auditing;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Logs.GetList;

public class Endpoint : EndpointWithoutRequest<List<AuditDto>>
{
    private readonly IAuditService _auditService;

    public Endpoint(IAuditService auditService)
    {
        _auditService = auditService;
    }

    public override void Configure()
    {
        Get("/logs");
        Summary(s =>
        {
            s.Summary = "Get list of all logs.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.AuditLog));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await _auditService.GetListAsync(ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}