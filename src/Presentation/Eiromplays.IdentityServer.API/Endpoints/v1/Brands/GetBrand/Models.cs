using Eiromplays.IdentityServer.Application.Catalog.Brands;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.GetBrand;

public class Models
{
    public class Request
    {
        public Guid Id { get; set; }
    }

    public class Response
    {
        public BrandDto Brand { get; set; }
    }
}