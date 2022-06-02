namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.DeleteRole;

public class Models
{
    public class Request
    {
        public string Id { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}