using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.Update;

public class Models
{
    public class Request
    {
        public int Id { get; set; }
        public UpdateApiScopeRequest Data { get; set; } = default!;
    }
}