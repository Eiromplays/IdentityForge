using Eiromplays.IdentityServer.Application.Catalog.Brands;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.Search;

public class Models
{
    public class Request
    {
        public SearchBrandsRequest Data { get; set; } = default!;
    }
}