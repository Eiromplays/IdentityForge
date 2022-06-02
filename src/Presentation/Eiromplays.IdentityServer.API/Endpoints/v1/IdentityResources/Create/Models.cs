using Eiromplays.IdentityServer.Application.Identity.IdentityResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityResources.Create;

public class Models
{
    public class Request
    {
        public CreateIdentityResourceRequest Data { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}