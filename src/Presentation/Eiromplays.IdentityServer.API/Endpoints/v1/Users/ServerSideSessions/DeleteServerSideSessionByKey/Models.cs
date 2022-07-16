namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ServerSideSessions.DeleteServerSideSessionByKey;

public class Models
{
    public class Request
    {
        public string Key { get; set; } = default!;
    }

    public class Response
    {
        public string Message { get; set; } = default!;
    }
}