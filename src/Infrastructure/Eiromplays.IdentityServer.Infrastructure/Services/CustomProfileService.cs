using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Services;

public class CustomProfileService : ProfileService<ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomProfileService(UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory) : base(userManager, claimsFactory)
    {
        _userManager = userManager;
    }

    public CustomProfileService(UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<ProfileService<ApplicationUser>> logger) :
        base(userManager, claimsFactory, logger)
    {
        _userManager = userManager;
    }
    
    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        await base.GetProfileDataAsync(context);
        
        var user = await _userManager.GetUserAsync(context.Subject);

        var claims = new List<Claim>();
        
        if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
            claims.Add(new Claim(JwtClaimTypes.Picture, user.ProfilePicture));

        context.IssuedClaims.AddRange(claims);
    }
}