using Eiromplays.IdentityServer.API.Grants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Grants.GetGrant;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
    }
    
    public class Response
    {
        public GrantsViewModel Grants { get; set; }
    }
}