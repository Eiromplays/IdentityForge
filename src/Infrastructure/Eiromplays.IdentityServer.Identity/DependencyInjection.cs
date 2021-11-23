using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Identity.Entities;
using Eiromplays.IdentityServer.Identity.Services;
using Eiromplays.IdentityServer.Infrastructure.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eiromplays.IdentityServer.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityDbContext>(options =>
                    options.UseInMemoryDatabase("CleanArchitectureDb"));
            }
            else
            {
                services.AddDbContext<IdentityDbContext>(options =>
                    options.UseNpgsql(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));
            }

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>() 
                .AddEntityFrameworkStores<IdentityDbContext>();

            services.AddIdentityServer();
            
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });

            return services;
        }
    }
}