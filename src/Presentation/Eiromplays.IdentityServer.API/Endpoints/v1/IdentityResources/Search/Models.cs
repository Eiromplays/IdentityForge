using Eiromplays.IdentityServer.Application.Identity.IdentityResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityResources.Search;

public class Models
{
    public class Request
    {
        public IdentityResourceListFilter Data { get; set; } = default!;
    }
}