using Microsoft.AspNetCore.Http;

namespace Eiromplays.IdentityServer.Infrastructure.Common.Extensions;

public static class HttpContextExtensions
{
    public static string GetOriginFromRequest(this HttpContext httpContext) =>
        $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}{httpContext.Request.PathBase.Value}";
}