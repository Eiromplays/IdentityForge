// Original Source code: https://github.com/dj-nitehawk/FastEndpoints

namespace Eiromplays.IdentityServer.Infrastructure.Common.Extensions;

public static class FastEndpointsExtensions
{
    internal static string EndpointName(this Type epType, string? verb = null, int? routeNum = null, bool shortEpNames = false)
    {
        var vrb = verb != null ? verb[0] + verb[1..].ToLowerInvariant() : null;
        var ep = shortEpNames ? epType.Name : epType.FullName!.Replace(".", string.Empty);
        return $"{vrb}{ep}{routeNum}";
    }
}