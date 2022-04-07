using Eiromplays.IdentityServer.Application.Catalog.Brands;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.UpdateBrand;

public class Models
{
    public class Request
    {
        public Guid Id { get; set; }
        public UpdateBrandRequest UpdateBrandRequest { get; set; }
    }
}