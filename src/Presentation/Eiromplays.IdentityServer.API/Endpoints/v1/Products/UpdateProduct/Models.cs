using Eiromplays.IdentityServer.Application.Catalog.Products;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.UpdateProduct;

public class Models
{
    public class Request
    {
        public Guid Id { get; set; }
        public UpdateProductRequest UpdateProductRequest { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
    }
}