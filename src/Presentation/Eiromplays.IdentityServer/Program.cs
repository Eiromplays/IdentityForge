using System.Collections.Immutable;
using Duende.IdentityServer;
using Eiromplays.IdentityServer.Application;
using Eiromplays.IdentityServer.Configurations;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure;
using Eiromplays.IdentityServer.Infrastructure.Common;
using FluentValidation.AspNetCore;
using Honeycomb.OpenTelemetry;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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

    builder.Services.AddInfrastructure(builder.Configuration, ProjectType.IdentityServer);
    
    builder.Services.AddApplication();

    builder.Services.AddControllersWithViews().AddFluentValidation();

    builder.Services.Configure<ApiBehaviorOptions>(options =>
        options.SuppressModelStateInvalidFilter = true);

    var app = builder.Build();
    
    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration, ProjectType.IdentityServer);

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