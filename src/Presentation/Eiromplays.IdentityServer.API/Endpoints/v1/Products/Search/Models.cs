using Eiromplays.IdentityServer.Application.Catalog.Products;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.Search;

public class Models
{
    public class Request
    {
        public SearchProductsRequest SearchProductsRequest { get; set; }
    }
}