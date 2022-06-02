using Eiromplays.IdentityServer.Application.Identity.ApiResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiResources.Search;

public class Models
{
    public class Request
    {
        public ApiResourceListFilter Data { get; set; } = default!;
    }
}