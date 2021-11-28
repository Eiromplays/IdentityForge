using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Application.Identity.DTOs.Role;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Eiromplays.IdentityServer.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eiromplays.IdentityServer.Infrastructure.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure<TKey, TUserDto, TRoleDto, TUserClaimDto, TRoleClaimDto, TUserLoginDto, TUser, TRole, TIdentityDbContext>(this IServiceCollection services, ConfigurationManager configurationManager)
        where TKey : IEquatable<TKey>
        where TUserDto : UserDto<TKey>
        where TRoleDto : RoleDto<TKey>
        where TUserClaimDto : UserClaimDto<TKey>
        where TRoleClaimDto : RoleClaimDto<TKey>
        where TUserLoginDto : UserLoginDto<TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TIdentityDbContext : DbContext
    {
        if (configurationManager.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitectureDb"));
        }
        else
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(
                    configurationManager.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));
        }

        services.AddDefaultIdentity<TUser>()
            .AddRoles<TRole>()
            .AddEntityFrameworkStores<TIdentityDbContext>();

        services.AddIdentityServer();

        services.AddTransient<IIdentityService<TUserDto, TRoleDto>, IdentityService<TKey, TUserDto, TRoleDto, TUserClaimDto, TRoleClaimDto, TUserLoginDto, TUser, TRole>>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
        });

        return services;
    }
}