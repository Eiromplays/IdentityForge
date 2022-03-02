using Eiromplays.IdentityServer.Application;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Remove default logging providers
builder.Logging.ClearProviders();

// Serilog configuration
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Register Serilog
builder.Logging.AddSerilog(logger);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration, ProjectType.Api);

// Add services to the container.

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "token";
        options.DefaultChallengeScheme = "token";
        options.DefaultAuthenticateScheme = "token";
    })
    .AddJwtBearer("token", options =>
    {
        options.Authority = "https://localhost:7001";
        options.Audience = "api";

        options.MapInboundClaims = false;
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
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

builder.Services
    .AddSwaggerDoc(maxEndpointVersion: 1, settings: s =>
    {
        s.DocumentName = "Release 1.0";
        s.Title = "Eiromplays IdentityServer API";
        s.Version = "v1.0";
    });

var app = builder.Build();

await app.Services.ApplyMigrationsAsync(app.Configuration);

app.UseSecurityHeaders(app.Configuration);
app.UseDefaultExceptionHandler();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(config =>
{
    config.GlobalEndpointOptions = (_, routeHandlerBuilder) =>
    {
        routeHandlerBuilder.RequireAuthorization("RequireInteractiveUser");
    };
    config.VersioningOptions = options =>
    {
        options.Prefix = "v";
        options.SuffixedVersion = false;
        options.DefaultVersion = 1;
    };
});
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(s => s.ConfigureDefaults());
}

await app.RunAsync();