using Eiromplays.IdentityServer.Application.Identity;
using Eiromplays.IdentityServer.Filters;
using Eiromplays.IdentityServer.Infrastructure.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add custom json configuration files

builder.Configuration.AddJsonFile("identitydata.json", true, true);
builder.Configuration.AddJsonFile($"identitydata.{builder.Environment.EnvironmentName}.json", true,
    true);

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

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllersWithViews(options =>
        options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Customise default API behaviour
builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

await app.Services.ApplyMigrationsAsync(app.Configuration);

await app.Services.ApplySeedsAsync(app.Configuration);

app.UseCookiePolicy();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSecurityHeaders(app.Configuration);

app.UseStaticFiles();

app.UseIdentityServer();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

await app.RunAsync();