using Eiromplays.IdentityServer.Application.Common.Configurations.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.Initialization;

internal class ApplicationDbSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<ApplicationDbSeeder> _logger;
    private IdentityData _identityData;

    public ApplicationDbSeeder(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager,
        CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger,
        IOptions<IdentityData> identityData)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _seederRunner = seederRunner;
        _logger = logger;
        _identityData = identityData.Value;
    }

    public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        await SeedRolesAsync(dbContext);
        await SeedAdminUserAsync();
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
                _logger.LogInformation("Seeding {role}.", dataRole.Name);
                role = new ApplicationRole(dataRole.Name, dataRole.Description);
                await _roleManager.CreateAsync(role);
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
            
            _logger.LogInformation("Seeding {role} Permission '{permission}'.", role.Name, permission.Name);
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

    private async Task SeedAdminUserAsync()
    {
        foreach (var dataUser in _identityData.Users)
        {
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == dataUser.Email && u.UserName == dataUser.UserName)
                is not { } user)
            {
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

                _logger.LogInformation("Seeding Default Admin User for.");
                await _userManager.CreateAsync(user, dataUser.Password);
                await _userManager.AddToRolesAsync(user, dataUser.Roles);
            }
        }
    }
}