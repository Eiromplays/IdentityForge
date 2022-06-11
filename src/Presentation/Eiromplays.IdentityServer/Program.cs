using Eiromplays.IdentityServer.Application;
using Eiromplays.IdentityServer.Configurations;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure;
using Eiromplays.IdentityServer.Infrastructure.Common;
using FluentValidation.AspNetCore;
using Newtonsoft.Json;
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

    builder.Services.AddControllers();
    
    builder.Services.AddFastEndpoints()
        .AddFluentValidation();

    var app = builder.Build();
    
    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration, ProjectType.IdentityServer, config =>
    {
        config.RoutingOptions = options => options.Prefix = "api";
        config.VersioningOptions = options =>
        {
            options.Prefix = "v";
            options.SuffixedVersion = false;
            options.DefaultVersion = 1;
        };
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