using Eiromplays.IdentityServer.Application.Catalog.Brands;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Brands.DeleteBrand;

public class Models
{
    public class Request
    {
        public Guid Id { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
    }
}