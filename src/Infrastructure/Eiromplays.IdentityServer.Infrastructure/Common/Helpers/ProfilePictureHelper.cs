using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

namespace Eiromplays.IdentityServer.Infrastructure.Common.Helpers;

public class ProfilePictureHelper
{
    public static string GetProfilePicture(ApplicationUser user, AccountConfiguration? accountConfiguration = null)
    {
        if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
        {
            return user.ProfilePicture.StartsWith(accountConfiguration?.ProfilePictureConfiguration.DefaultUrl ?? string.Empty)
                ? user.ProfilePicture
                : $"{accountConfiguration?.ProfilePictureConfiguration.BaseUrl}{user.ProfilePicture}";
        }

        string? email = user.GravatarEmail ?? user.Email;

        return GetGravatarUrl(email);
    }

    private static string GetGravatarUrl(string? email, int? size = 150)
    {
        string hash = Md5HashHelper.GetMd5Hash(email);

        string sizeArgument = size > 0 ? $"?s={size}" : string.Empty;

        return $"https://www.gravatar.com/avatar/{hash}{sizeArgument}";
    }
}