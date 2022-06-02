namespace Eiromplays.IdentityServer.API.Endpoints.v1.UserLogins.GetUserSessionByKey;

public class Models
{
    public class Request
    {
        public string Key { get; set; } = default!;
    }
}