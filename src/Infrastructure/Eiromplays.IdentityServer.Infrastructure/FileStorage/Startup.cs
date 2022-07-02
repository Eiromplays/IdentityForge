using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Eiromplays.IdentityServer.Infrastructure.FileStorage;

internal static class Startup
{
    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app)
    {
        string staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        if (!Directory.Exists(staticFilesPath))
        {
            Directory.CreateDirectory(staticFilesPath);
        }

        return app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilesPath),
            RequestPath = new PathString("/Files")
        });
    }
}