using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts.Seeds;

public static class IdentityDbContextSeed
{
    public static async Task SeedDefaultUsersAndRolesAsync(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager, IdentityData identityData)
    {
        foreach (var role in identityData.Roles.Select(role => new ApplicationRole { Name = role.Name }))
        {
            if (await roleManager.Roles.AllAsync(r => r.Name != role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }

        foreach (var user in identityData.Users)
        {
            var applicationUser = new ApplicationUser
            {
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = true
            };

            if (!await userManager.Users.AllAsync(u => u.UserName != user.UserName)) continue;

            var userIdentityResult = await userManager.CreateAsync(applicationUser, user.Password);
            if (!userIdentityResult.Succeeded)
                return;

            await userManager.AddToRolesAsync(applicationUser, user.Roles);
        }
    }
}