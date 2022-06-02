using Eiromplays.IdentityServer.Application.Catalog.Products;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.UpdateProduct;

public class Models
{
    public class Request
    {
        public Guid Id { get; set; } = default;
        public UpdateProductRequest Data { get; set; } = default!;
    }
}