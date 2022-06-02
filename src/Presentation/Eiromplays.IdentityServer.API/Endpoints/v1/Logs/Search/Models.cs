using Eiromplays.IdentityServer.Application.Auditing;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Logs.Search;

public class Models
{
    public class Request
    {
        public AuditLogListFilter Data { get; set; } = default!;
    }
}