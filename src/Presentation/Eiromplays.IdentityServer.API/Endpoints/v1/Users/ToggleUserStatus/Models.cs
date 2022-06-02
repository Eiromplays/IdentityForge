using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ToggleUserStatus;

public class Models
{
    public class Request
    {
        public string Id { get; set; } = default!;
        public ToggleUserStatusRequest Data { get; set; } = default!;
    }
}