using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Identity.Persistence.DbContexts;
using Eiromplays.IdentityServer.Infrastructure.Identity.Services;
using Eiromplays.IdentityServer.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
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

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
        });

        return services;
    }
}