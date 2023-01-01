using Duende.Bff.Yarp;
using Eiromplays.IdentityServer.Admin.WebUI.Configurations;
using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure;
using Eiromplays.IdentityServer.Infrastructure.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Logging;
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

    builder.Services.AddAuthorization();
    builder.Services.AddInfrastructure(builder.Configuration, ProjectType.Spa);

    IdentityModelEventSource.ShowPII = true;

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    }).AddCookie("cookie", options =>
    {
        options.Cookie.Name = "__Host-bff";

        // add an instance of the patched manager to the options:
        options.CookieManager = new ChunkingCookieManager();

        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    }).AddOpenIdConnect("oidc", options =>
    {
        var spaOpenIdConnectOptionsSection = builder.Configuration.GetSection(nameof(SpaOpenIdConnectOptions));

        spaOpenIdConnectOptionsSection.Bind(options);

        var spaOpenIdConnectOptions = spaOpenIdConnectOptionsSection.Get<SpaOpenIdConnectOptions>();
        foreach (var spaClaimAction in spaOpenIdConnectOptions.ClaimActions)
        {
            options.ClaimActions.Add(spaClaimAction.MapToClaimAction());
        }
    });

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");

        // The default HSTS0+ value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();

    app.UseBff();

    app.UseAuthorization();

    app.MapBffManagementEndpoints();

    app.MapBffReverseProxy(options =>
    {
        options.UseAntiforgeryCheck();
    });

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