using Eiromplays.IdentityServer.Application.Identity.ApiScopes;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiScopes.Create;

public class Models
{
    public class Request
    {
        public CreateApiScopeRequest Data { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}