using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

namespace Eiromplays.IdentityServer.Infrastructure.Common.Helpers;

public class ProfilePictureHelper
{
    public static string GetProfilePicture(ApplicationUser user, AccountConfiguration? accountConfiguration = null, string? baseProfilePictureUrl = null)
    {
        if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
        {
            if (Uri.TryCreate(user.ProfilePicture, UriKind.Absolute, out var profilePictureUri) &&
                (profilePictureUri.Scheme == Uri.UriSchemeHttp || profilePictureUri.Scheme == Uri.UriSchemeHttps))
            {
                return profilePictureUri.ToString();
            }

            return
                $"{accountConfiguration?.ProfilePictureConfiguration.BaseUrl ?? baseProfilePictureUrl}{user.ProfilePicture}";
        }

        string email = user.GravatarEmail ?? user.Email;

        return GetGravatarUrl(email);
    }

    private static string GetGravatarUrl(string? email, int? size = 150)
    {
        string hash = Md5HashHelper.GetMd5Hash(email);

        string sizeArgument = size > 0 ? $"?s={size}" : string.Empty;

        return $"https://www.gravatar.com/avatar/{hash}{sizeArgument}";
    }
}