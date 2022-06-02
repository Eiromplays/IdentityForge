using Eiromplays.IdentityServer.Application.Catalog.Brands;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.CreateBrand;

public class Models
{
    public class Request
    {
        public CreateBrandRequest Data { get; set; } = default!;
    }
}