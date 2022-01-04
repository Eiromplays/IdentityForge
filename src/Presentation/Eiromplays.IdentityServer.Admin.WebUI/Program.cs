using Eiromplays.IdentityServer.Admin.WebUI.Filters;
using Eiromplays.IdentityServer.Admin.WebUI.Services;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Eiromplays.IdentityServer.Infrastructure.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication();

builder.Services
    .AddInfrastructure(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews(options =>
        options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

builder.Services.AddEndpointDefinitions(typeof(UserDto));

builder.Services.AddRazorPages();

// In production, the React files will be served from this directory
if (builder.Environment.IsProduction())
    builder.Services.AddSpaStaticFiles(configuration =>
        configuration.RootPath = "ClientApp/build");

var app = builder.Build();

app.UseEndpointDefinitions();

// Configure the HTTP request pipeline.
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

app.UseStaticFiles();
if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.UseSpa(spa =>
{
    // To learn more about options for serving an React SPA from ASP.NET Core,
    // see https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/react?view=aspnetcore-6.0&tabs=visual-studio

    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        //spa.UseReactDevelopmentServer(npmScript: "start");
        spa.UseProxyToSpaDevelopmentServer(app.Configuration["SpaBaseUrl"] ?? "http://localhost:4200");
    }
});

app.MapFallbackToFile("index.html");

app.Run();