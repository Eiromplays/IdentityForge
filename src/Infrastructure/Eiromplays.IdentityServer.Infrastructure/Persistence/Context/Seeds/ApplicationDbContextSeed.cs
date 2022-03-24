using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Claim = System.Security.Claims.Claim;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Context.Seeds;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUsersAndRolesAsync(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager, IdentityData identityData)
    {
        foreach (var role in identityData.Roles)
        {
            if (!await roleManager.Roles.AllAsync(r => r.Name != role.Name)) continue;

            var applicationRole = new ApplicationRole(role.Name, role.Description);
            var roleIdentityResult = await roleManager.CreateAsync(applicationRole);                
            if (!roleIdentityResult.Succeeded) continue;

            foreach (var claim in role.Claims.Select(claim => new Claim(claim.Type, claim.Value)))
            {
                await roleManager.AddClaimAsync(applicationRole, claim);
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

            if (!await userManager.Users.AllAsync(u => u.UserName != user.UserName || u.Email == user.Email)) continue;

            var userIdentityResult = await userManager.CreateAsync(applicationUser, user.Password);
            if (!userIdentityResult.Succeeded)
                return;

            await userManager.AddToRolesAsync(applicationUser, user.Roles);

            await userManager.AddClaimsAsync(applicationUser, user.Claims.Select(claim => new Claim(claim.Type, claim.Value)));
        }
    }
}