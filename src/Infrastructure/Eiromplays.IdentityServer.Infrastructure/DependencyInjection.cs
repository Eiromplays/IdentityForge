using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Infrastructure.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity.Services;
using Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eiromplays.IdentityServer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
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

            services.AddScoped<IIdentityDbContext>(provider => provider.GetService<IdentityDbContext>()!);

            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });

            return services;
        }
    }
}