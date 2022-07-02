using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUserRoles;

public class Models
{
    public class Request
    {
        public string Id { get; set; } = default!;

        public UserRolesRequest UserRolesRequest { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}