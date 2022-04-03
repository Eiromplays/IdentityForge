using Eiromplays.IdentityServer.Application.Catalog.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.Export;

public class Models
{
    public class Request
    {
        public ExportProductsRequest ExportProductsRequest { get; set; }
    }

    public class Response
    {
        public FileResult FileResult { get; set; }
    }
}