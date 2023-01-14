using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Infrastructure.Common.Helpers;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

public class CustomProfileService : ProfileService<ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AccountConfiguration _accountConfiguration;
    private readonly IStringLocalizer _t;

    public CustomProfileService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptions<AccountConfiguration> accountConfigurationOptions,
        IStringLocalizer<CustomProfileService> t)
        : base(userManager, claimsFactory)
    {
        _userManager = userManager;
        _t = t;
        _accountConfiguration = accountConfigurationOptions.Value;
    }

    public CustomProfileService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        ILogger<CustomProfileService> logger,
        IOptions<AccountConfiguration> accountConfigurationOptions,
        IStringLocalizer<CustomProfileService> t)
        : base(userManager, claimsFactory, logger)
    {
        _userManager = userManager;
        _t = t;
        _accountConfiguration = accountConfigurationOptions.Value;
    }

    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        await base.GetProfileDataAsync(context);

        var user = await _userManager.GetUserAsync(context.Subject);

        if (user is null)
        {
            throw new InternalServerException(_t["No valid user provided to profile service"]);
        }

        var claims = new List<Claim>();

        string profilePicture = ProfilePictureHelper.GetProfilePicture(user, _accountConfiguration);

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            claims.Add(new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));

        if (user.PhoneNumberConfirmed)
            claims.Add(new Claim(JwtClaimTypes.PhoneNumberVerified, "true", ClaimValueTypes.Boolean));

        if (!string.IsNullOrEmpty(user.FirstName))
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

        if (!string.IsNullOrEmpty(user.LastName))
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));

        if (!string.IsNullOrWhiteSpace(profilePicture))
            claims.Add(new Claim(JwtClaimTypes.Picture, profilePicture));

        if (!string.IsNullOrWhiteSpace(user.GravatarEmail))
            claims.Add(new Claim("gravatar_email", user.GravatarEmail));

        if (user.LastModifiedOn is not null)
            claims.Add(new Claim("updated_at", user.LastModifiedOn.Value.ToString(CultureInfo.InvariantCulture)));

        claims.Add(new Claim("created_at", user.CreatedOn.ToString(CultureInfo.InvariantCulture)));

        context.IssuedClaims.AddRange(claims);
    }
}