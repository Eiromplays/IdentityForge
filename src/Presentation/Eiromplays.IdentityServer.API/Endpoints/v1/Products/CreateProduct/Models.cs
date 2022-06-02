using Eiromplays.IdentityServer.Application.Catalog.Products;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.CreateProduct;

public class Models
{
    public class Request
    {
        public CreateProductRequest Data { get; set; } = default!;
    }
}