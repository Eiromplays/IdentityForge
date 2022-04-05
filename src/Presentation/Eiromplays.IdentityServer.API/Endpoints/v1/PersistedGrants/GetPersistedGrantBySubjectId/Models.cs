using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.GetPersistedGrantBySubjectId;

public class Models
{
    public class Request
    {
        public string? SubjectId { get; set; }
    }
    
    public class Response
    {
        public List<PersistedGrantDto> PersistedGrants { get; set; }
    }
}