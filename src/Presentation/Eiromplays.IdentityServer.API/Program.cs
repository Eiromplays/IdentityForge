using Eiromplays.IdentityServer.API.Filters;
using Eiromplays.IdentityServer.Application.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
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

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllersWithViews(options =>
        options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = ApiVersion.Default;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new MediaTypeApiVersionReader("version"),
        new HeaderApiVersionReader("X-Version")
    );

    options.ReportApiVersions = true;
});

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
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Customise default API behaviour
builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

await app.Services.ApplyMigrationsAsync(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSecurityHeaders(app.Configuration);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers()
        .RequireAuthorization("ApiCaller");
});

await app.RunAsync();