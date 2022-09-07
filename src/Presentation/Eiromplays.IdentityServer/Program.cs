using Eiromplays.IdentityServer.Application;
using Eiromplays.IdentityServer.Configurations;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure;
using Eiromplays.IdentityServer.Infrastructure.Common;
using FluentValidation.AspNetCore;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.AddConfigurations();
    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
            .ReadFrom.Configuration(builder.Configuration);
    });

    builder.Services.AddInfrastructure(builder.Configuration, builder.Environment,  ProjectType.IdentityServer);

    builder.Services.AddApplication();

    builder.Services.AddControllersWithViews();

    builder.Services.AddFastEndpoints()
        .AddFluentValidationAutoValidation();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync().ConfigureAwait(false);

    app.UseInfrastructure(builder.Configuration, ProjectType.IdentityServer, config =>
    {
        config.Endpoints.RoutePrefix = "api";
        config.Versioning.Prefix = "v";
        config.Versioning.DefaultVersion = 1;
        config.Versioning.PrependToRoute = true;
    });

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapDefaultControllerRoute();
    });

    app.MapEndpoints();

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}