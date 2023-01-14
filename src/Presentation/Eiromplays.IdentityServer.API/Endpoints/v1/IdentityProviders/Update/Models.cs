using Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityProviders.Update;

public class Models
{
    public class Request
    {
        public int Id { get; set; }
        public UpdateIdentityProviderRequest Data { get; set; } = default!;
    }
}