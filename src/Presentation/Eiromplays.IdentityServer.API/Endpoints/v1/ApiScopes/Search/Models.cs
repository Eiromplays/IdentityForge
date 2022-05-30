using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.Search;

public class Models
{
    public class Request
    {
        public ApiScopeListFilter Data { get; set; } = default!;
    }
}