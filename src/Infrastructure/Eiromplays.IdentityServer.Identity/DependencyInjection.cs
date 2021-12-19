using Eiromplays.IdentityServer.Application.Identity.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Identity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eiromplays.IdentityServer.Infrastructure.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        if (configurationManager.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<Persistence.DbContexts.IdentityDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitectureDb"));
        }
        else
        {
            services.AddDbContext<Persistence.DbContexts.IdentityDbContext>(options =>
                options.UseNpgsql(
                    configurationManager.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(Persistence.DbContexts.IdentityDbContext).Assembly.FullName)));
        }

        services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<Persistence.DbContexts.IdentityDbContext>();

        services.AddIdentityServer();

        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
        });

        return services;
    }
}