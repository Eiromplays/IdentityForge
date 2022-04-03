using Eiromplays.IdentityServer.Application.Auditing;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetLogs;

public class Models
{
    public class Request
    {
    }
    
    public class Response
    {
        public List<AuditDto> Logs { get; set; } = new();
    }
}