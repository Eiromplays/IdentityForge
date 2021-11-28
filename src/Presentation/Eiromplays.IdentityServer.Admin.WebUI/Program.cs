using Eiromplays.IdentityServer.Application.Identity;
using Eiromplays.IdentityServer.Application.Identity.DTOs.Role;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Eiromplays.IdentityServer.Infrastructure.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication();

builder.Services
    .AddInfrastructure<string, UserDto<string>, RoleDto<string>, UserClaimDto<string>, RoleClaimDto<string>,
        UserLoginDto<string>, ApplicationUser, ApplicationRole, IdentityDbContext>(builder.Configuration);

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
