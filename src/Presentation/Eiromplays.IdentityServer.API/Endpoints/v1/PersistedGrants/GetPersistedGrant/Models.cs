using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.GetPersistedGrant;

public class Models
{
    public class Request
    {
        public string? Key { get; set; }
    }
    
    public class Response
    {
        public PersistedGrantDto PersistedGrant { get; set; }
    }
}