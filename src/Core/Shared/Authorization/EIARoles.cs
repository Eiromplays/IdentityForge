namespace Shared.Authorization;

using System.Collections.ObjectModel;

// ReSharper disable once InconsistentNaming
public static class EIARoles
{
    public const string Administrator = nameof(Administrator);
    public const string Basic = nameof(Basic);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Administrator,
        Basic
    });

    public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
}