namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.GetPermissions;

public class Models
{
    public class Request
    {
    }
    
    public class Response
    {
        public List<string> Permissions { get; set; } = new();
    }
}