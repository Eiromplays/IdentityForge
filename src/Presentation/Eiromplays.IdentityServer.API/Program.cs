using Eiromplays.IdentityServer.API.Configurations;
using Eiromplays.IdentityServer.Application;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure;
using FluentValidation.AspNetCore;
using Serilog;
using Eiromplays.IdentityServer.Infrastructure.Common;
using FluentEmail.Core;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.AddConfigurations();
    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
            .ReadFrom.Configuration(builder.Configuration);
    });

    builder.Services.AddControllers();

    builder.Services.AddInfrastructure(builder.Configuration, builder.Environment, ProjectType.Api);

    builder.Services.AddApplication();
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "token";
            options.DefaultChallengeScheme = "token";
            options.DefaultAuthenticateScheme = "token";
        })
        .AddJwtBearer("token", options =>
        {
            options.Authority = "http://auth.eiromplays.local.com";
            options.Audience = "api";
            options.MapInboundClaims = false;
            options.RequireHttpsMetadata = false;
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("ApiCaller", policy =>
        {
            policy.RequireClaim("scope", "api");
        });

        options.AddPolicy("RequireInteractiveUser", policy =>
        {
            policy.RequireClaim("sub");
        });

        options.AddPolicy("RequireAdministrator", policy =>
        {
            policy.RequireClaim("role", "Administrator");
        });
    });

    builder.Services.AddFastEndpoints()
        .AddFluentValidationAutoValidation();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration, ProjectType.Api, config =>
    {
        config.Endpoints.Configurator = ep =>
        {
            ep.Options(b => b.RequireAuthorization("RequireInteractiveUser"));
        };
        config.Versioning.Prefix = "v";
        config.Versioning.DefaultVersion = 1;
        config.Versioning.PrependToRoute = true;
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
