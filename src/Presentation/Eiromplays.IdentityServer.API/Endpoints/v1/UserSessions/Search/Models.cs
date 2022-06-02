using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.UserLogins.Search;

public class Models
{
    public class Request
    {
        public UserSessionListFilter Data { get; set; } = default!;
    }
}