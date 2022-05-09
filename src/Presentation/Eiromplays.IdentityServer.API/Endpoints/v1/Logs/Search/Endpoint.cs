using Eiromplays.IdentityServer.Application.Auditing;
using Eiromplays.IdentityServer.Application.Common.Models;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Logs.Search;

public class Endpoint : Endpoint<Models.Request, PaginationResponse<AuditDto>>
{
    private readonly IAuditService _auditService;
    
    public Endpoint(IAuditService auditService)
    {
        _auditService = auditService;
    }

    public override void Configure()
    {
        Post("/logs/search");
        Summary(s =>
        {
            s.Summary = "Search logs using available filters.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Search, EIAResource.AuditLog));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _auditService.SearchAsync(request.Data, ct);

        await SendAsync(Response, cancellation: ct);
    }
}