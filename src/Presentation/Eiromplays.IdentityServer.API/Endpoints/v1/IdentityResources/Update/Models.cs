using Eiromplays.IdentityServer.Application.Identity.IdentityResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityResources.Update;

public class Models
{
    public class Request
    {
        public int Id { get; set; }
        public UpdateIdentityResourceRequest Data { get; set; } = default!;
    }
}