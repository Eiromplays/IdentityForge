namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.Delete;

public class Models
{
    public class Request
    {
        public string? Key { get; set; }
    }

    public class Response
    {
        public string Message { get; set; }
    }
}