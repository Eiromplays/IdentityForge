using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.Search;

public class Models
{
    public class Request
    {
        public RoleListFilter Data { get; set; } = default!;
    }
}