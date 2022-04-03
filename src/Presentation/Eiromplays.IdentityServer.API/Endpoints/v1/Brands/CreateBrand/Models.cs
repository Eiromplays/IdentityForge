using Eiromplays.IdentityServer.Application.Catalog.Brands;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.CreateBrand;

public class Models
{
    public class Request
    {
        public CreateBrandRequest CreateBrandRequest { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
    }
}