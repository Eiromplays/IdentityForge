using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

namespace Eiromplays.IdentityServer.Infrastructure.Common.Helpers;

public class ProfilePictureHelper
{
    public static string GetProfilePicture(ApplicationUser? user, AccountConfiguration? accountConfiguration = null, string? baseProfilePictureUrl = null)
    {
        if (!string.IsNullOrWhiteSpace(user?.ProfilePicture))
        {
            if (Uri.TryCreate(user.ProfilePicture, UriKind.Absolute, out var profilePictureUri) &&
                (profilePictureUri.Scheme == Uri.UriSchemeHttp || profilePictureUri.Scheme == Uri.UriSchemeHttps))
            {
                return profilePictureUri.ToString();
            }

            if (Uri.TryCreate(
                    new Uri(accountConfiguration?.ProfilePictureConfiguration.BaseUrl ?? baseProfilePictureUrl ?? string.Empty),
                    user.ProfilePicture,
                    out var fullProfilePictureUri) &&
                (fullProfilePictureUri.Scheme == Uri.UriSchemeHttp ||
                 fullProfilePictureUri.Scheme == Uri.UriSchemeHttps))
            {
                return fullProfilePictureUri.ToString();
            }
        }

        string? email = user?.GravatarEmail ?? user?.Email;

        return GetGravatarUrl(email);
    }

    private static string GetGravatarUrl(string? email, int? size = 150)
    {
        string hash = Md5HashHelper.GetMd5Hash(email);

        string sizeArgument = size > 0 ? $"?s={size}" : string.Empty;

        return $"https://www.gravatar.com/avatar/{hash}{sizeArgument}";
    }
}