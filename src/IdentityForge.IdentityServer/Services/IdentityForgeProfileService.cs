using System.Security.Claims;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using IdentityForge.IdentityServer.Configurations;
using IdentityForge.IdentityServer.Domain.Users;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityForge.IdentityServer.Services;

public class IdentityForgeProfileService : ProfileService<ApplicationUser>
{
    private readonly AccountConfiguration _accountConfiguration;


    public IdentityForgeProfileService(UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<AccountConfiguration> accountConfiguration) : base(
        userManager, claimsFactory)
    {
        _accountConfiguration = accountConfiguration.Value;
    }

    public IdentityForgeProfileService(UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<ProfileService<ApplicationUser>> logger,
        IOptions<AccountConfiguration> accountConfiguration) : base(userManager, claimsFactory, logger)
    {
        _accountConfiguration = accountConfiguration.Value;
    }

    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        await base.GetProfileDataAsync(context).ConfigureAwait(false);

        var user = await FindUserAsync(context.Subject.GetSubjectId()).ConfigureAwait(false);

        var claims = new List<Claim>();

        string profilePicture = user.GetProfilePicture(_accountConfiguration, "");

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
            claims.Add(new Claim(JwtClaimTypes.UpdatedAt, user.LastModifiedOn.Value.ToString("O"),
                ClaimValueTypes.Integer64));

        claims.Add(new Claim("created_at", user.CreatedOn.ToString("O"),
            ClaimValueTypes.Integer64));

        await base.GetProfileDataAsync(context).ConfigureAwait(false);

        context.AddRequestedClaims(claims);
    }
}