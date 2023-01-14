using System.Collections.ObjectModel;

namespace Shared.Authorization;

// ReSharper disable once InconsistentNaming
public static class EIARoles
{
    public const string Administrator = nameof(Administrator);
    public const string Basic = nameof(Basic);

    public static IEnumerable<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Administrator,
        Basic
    });

    public static bool IsDefault(string? roleName) => DefaultRoles.Any(r => r.Equals(roleName));
}