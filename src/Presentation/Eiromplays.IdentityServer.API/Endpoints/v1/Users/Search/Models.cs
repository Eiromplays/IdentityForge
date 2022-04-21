using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.Search;

public class Models
{
    public class Request
    {
        public UserListFilter Data { get; set; } = default!;
    }
}