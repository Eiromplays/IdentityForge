using System.Globalization;
using System.Security.Claims;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Infrastructure.Common.Helpers;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Services;

public class CustomProfileService : ProfileService<ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AccountConfiguration _accountConfiguration;

    public CustomProfileService(UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptionsMonitor<AccountConfiguration> accountConfigurationOptions) : base(userManager, claimsFactory)
    {
        _userManager = userManager;
        _accountConfiguration = accountConfigurationOptions.CurrentValue;
    }

    public CustomProfileService(UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<CustomProfileService> logger,
        IOptionsMonitor<AccountConfiguration> accountConfigurationOptions) :
        base(userManager, claimsFactory, logger)
    {
        _userManager = userManager;
        _accountConfiguration = accountConfigurationOptions.CurrentValue;
    }
    
    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        await base.GetProfileDataAsync(context);
        
        var user = await _userManager.GetUserAsync(context.Subject);

        var claims = new List<Claim>();

        var profilePicture = ProfilePictureHelper.GetProfilePicture(user, _accountConfiguration);
        
        if (!string.IsNullOrWhiteSpace(profilePicture))
            claims.Add(new Claim(JwtClaimTypes.Picture, profilePicture));

        if (!string.IsNullOrWhiteSpace(user.GravatarEmail))
            claims.Add(new Claim("gravatar_email", user.GravatarEmail));

        if (user.LastModified is not null)
            claims.Add(new Claim("updated_at", user.LastModified.Value.ToString(CultureInfo.InvariantCulture)));
        
        claims.Add(new Claim("created_at", user.Created.ToString(CultureInfo.InvariantCulture)));
        
        context.IssuedClaims.AddRange(claims);
    }
}