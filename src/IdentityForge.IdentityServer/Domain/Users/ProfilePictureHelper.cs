using IdentityForge.IdentityServer.Configurations;

namespace IdentityForge.IdentityServer.Domain.Users;

public static class ProfilePictureHelper
{
    public static string GetProfilePicture(this ApplicationUser? user, AccountConfiguration? accountConfiguration = null, string? baseProfilePictureUrl = null)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
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

        string? email = user.GravatarEmail ?? user.Email;

        if (!string.IsNullOrWhiteSpace(email))
            return GetGravatarUrl(email);

        return !string.IsNullOrWhiteSpace(accountConfiguration?.ProfilePictureConfiguration.DefaultUrl)
            ? $"{accountConfiguration?.ProfilePictureConfiguration.DefaultUrl}?seed={user.FullName}"
            : $"https://api.dicebear.com/7.x/initials/svg?seed={user.FullName}";
    }

    private static string GetGravatarUrl(string? email, int? size = 150)
    {
        string hash = Md5HashHelper.GetMd5Hash(email);

        string sizeArgument = size > 0 ? $"?s={size}" : string.Empty;

        return $"https://www.gravatar.com/avatar/{hash}{sizeArgument}";
    }

    public static string GetProfilePicture(
        this ApplicationUser user,
        AccountConfiguration? accountConfiguration = null,
        Uri? baseProfilePictureUrl = null) =>
        GetProfilePicture(user, accountConfiguration, baseProfilePictureUrl?.ToString());
}