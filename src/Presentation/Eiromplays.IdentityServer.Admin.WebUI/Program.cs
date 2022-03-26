using Eiromplays.IdentityServer.Application;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure;
using Eiromplays.IdentityServer.Infrastructure.Common;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
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
    
    builder.Services.AddInfrastructure(builder.Configuration, ProjectType.Api);
    
    builder.Services.AddApplication();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    }).AddCookie("cookie", options =>
    {
        options.Cookie.Name = "__Host-bff";
        options.Cookie.SameSite = SameSiteMode.Strict;
    }).AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:7001";
        options.ClientId = "eiromplays_identity_admin_spa";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;
        options.SaveTokens = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("api");
        options.Scope.Add("offline_access");
        options.Scope.Add("roles");
        options.Scope.Add("email");
    
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.ClaimActions.MapJsonKey("picture", "picture", "picture");
    });
    
    builder.Services.AddControllersWithViews().AddFluentValidation();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration, ProjectType.Api);
    app.MapEndpoints();
    
    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseBff();

    app.MapBffManagementEndpoints();

    app.MapRemoteBffApiEndpoint("/users", "https://localhost:7003/v1/users")
        .RequireAccessToken();

    app.MapRemoteBffApiEndpoint("/roles", "https://localhost:7003/v1/roles")
        .RequireAccessToken();

    app.MapFallbackToFile("index.html");

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