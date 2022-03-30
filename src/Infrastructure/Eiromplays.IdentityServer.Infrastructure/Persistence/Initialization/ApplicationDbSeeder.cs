using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Authorization;
using Claim = System.Security.Claims.Claim;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal class ApplicationDbSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<ApplicationDbSeeder> _logger;
    private readonly IdentityData _identityData;
    private readonly IdentityServerData _identityServerData;

    public ApplicationDbSeeder(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager,
        CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger,
        IOptions<IdentityData> identityData, IOptions<IdentityServerData> identityServerData)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _seederRunner = seederRunner;
        _logger = logger;
        _identityServerData = identityServerData.Value;
        _identityData = identityData.Value;
    }

    public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        await SeedRolesAsync(dbContext);
        await SeedUsersAsync();
        await SeedIdentityResources(dbContext);
        await SeedApiScopes(dbContext);
        await SeedClients(dbContext);
        await SeedApiResources(dbContext);
        await _seederRunner.RunSeedersAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(ApplicationDbContext dbContext)
    {
        foreach (var dataRole in _identityData.Roles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == dataRole.Name)
                is not { } role)
            {
                // Create the role
                _logger.LogInformation("Seeding {Role}", dataRole.Name);
                role = new ApplicationRole(dataRole.Name, dataRole.Description);
                
                await _roleManager.CreateAsync(role);
                
                foreach (var claim in dataRole.Claims.Select(claim => new Claim(claim.Type, claim.Value)))
                {
                    await _roleManager.AddClaimAsync(role, claim);
                }
            }

            switch (dataRole.Name)
            {
                // Assign permissions
                case EIARoles.Basic:
                    await AssignPermissionsToRoleAsync(dbContext, EIAPermissions.Basic, role);
                    break;
                case EIARoles.Admin:
                {
                    await AssignPermissionsToRoleAsync(dbContext, EIAPermissions.Admin, role);
                    
                    break;
                }
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<EIAPermission> permissions, ApplicationRole role)
    {
        var currentClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (currentClaims.Any(c => c.Type == EIAClaims.Permission && c.Value == permission.Name)) continue;
            
            _logger.LogInformation("Seeding {Role} Permission '{Permission}'", role.Name, permission.Name);
            dbContext.RoleClaims.Add(new ApplicationRoleClaim
            {
                RoleId = role.Id,
                ClaimType = EIAClaims.Permission,
                ClaimValue = permission.Name,
                CreatedBy = "ApplicationDbSeeder"
            });

            await dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedUsersAsync()
    {
        foreach (var dataUser in _identityData.Users)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.Email == dataUser.Email && u.UserName == dataUser.UserName);
            
            if (user is not null) continue;
            
            user = new ApplicationUser
            {
                FirstName = dataUser.FirstName,
                LastName = dataUser.LastName,
                Email = dataUser.Email,
                UserName = dataUser.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            _logger.LogInformation("Seeding User {UserName}", dataUser.UserName);
            
            await _userManager.CreateAsync(user, dataUser.Password);
            await _userManager.AddToRolesAsync(user, dataUser.Roles);
            await _userManager.AddClaimsAsync(user, dataUser.Claims.Select(claim => new Claim(claim.Type, claim.Value)));
        }
    }
    
    private async Task SeedIdentityResources(ApplicationDbContext dbContext)
    {
        foreach (var resource in _identityServerData.IdentityResources)
        {
            var exits = await dbContext.IdentityResources.AnyAsync(a => a.Name == resource.Name);

            if (exits)
            {
                continue;
            }
            
            _logger.LogInformation("Seeding {Resource} Resource", resource.Name);
            await dbContext.IdentityResources.AddAsync(resource.ToEntity());
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task SeedApiScopes(ApplicationDbContext dbContext)
    {
        foreach (var apiScope in _identityServerData.ApiScopes)
        {
            var exits = await dbContext.ApiScopes.AnyAsync(a => a.Name == apiScope.Name);

            if (exits)
            {
                continue;
            }

            await dbContext.ApiScopes.AddAsync(apiScope.ToEntity());
        }
        
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedApiResources(ApplicationDbContext dbContext)
    {
        foreach (var resource in _identityServerData.ApiResources)
        {
            var exits = await dbContext.ApiResources.AnyAsync(a => a.Name == resource.Name);

            if (exits)
            {
                continue;
            }

            foreach (var s in resource.ApiSecrets)
            {
                s.Value = s.Value.ToSha256();
            }

            await dbContext.ApiResources.AddAsync(resource.ToEntity());
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task SeedClients(ApplicationDbContext dbContext)
    {
        foreach (var client in _identityServerData.Clients)
        {
            var exits = await dbContext.Clients.AnyAsync(a => a.ClientId == client.ClientId);

            if (exits)
            {
                continue;
            }

            foreach (var secret in client.ClientSecrets)
            {
                secret.Value = secret.Value.ToSha256();
            }

            client.Claims = client.ClientClaims
                .Select(c => new ClientClaim(c.Type, c.Value))
                .ToList();

            await dbContext.Clients.AddAsync(client.ToEntity());
        }

        await dbContext.SaveChangesAsync();
    }
}