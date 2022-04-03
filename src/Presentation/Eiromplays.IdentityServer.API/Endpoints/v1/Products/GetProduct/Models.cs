using Eiromplays.IdentityServer.Application.Catalog.Brands;
using Eiromplays.IdentityServer.Application.Catalog.Products;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.GetProduct;

public class Models
{
    public class Request
    {
        public Guid Id { get; set; }
    }

    public class Response
    {
        public ProductDetailsDto ProductDetails { get; set; }
    }
}