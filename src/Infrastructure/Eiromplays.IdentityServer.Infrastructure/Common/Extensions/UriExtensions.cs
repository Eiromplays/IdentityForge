namespace Eiromplays.IdentityServer.Infrastructure.Common.Extensions;

public static class UriExtensions
{
    public static bool IsValidUri(this string urlToCheck)
    {
        return Uri.TryCreate(urlToCheck, UriKind.Absolute, out var profilePictureUri) &&
            (profilePictureUri.Scheme == Uri.UriSchemeHttp || profilePictureUri.Scheme == Uri.UriSchemeHttps);
    }
}