using Eiromplays.IdentityServer.Application.Catalog.Products;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.Export;

public class Models
{
    public class Request
    {
        public ExportProductsRequest Data { get; set; } = default!;
    }
}