using Eiromplays.IdentityServer.Application.Identity.Users.Logins;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Providers;

public class RemoveUserProviderModels
{
    public class Request
    {
        public string Id { get; set; } = default!;

        public RemoveLoginRequest RemoveLoginRequest { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}