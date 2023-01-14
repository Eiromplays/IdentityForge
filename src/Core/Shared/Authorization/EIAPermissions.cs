using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class EiaAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class EiaResource
{
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string UserClaims = nameof(UserClaims);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string PersistedGrants = nameof(PersistedGrants);
    public const string AuditLog = nameof(AuditLog);
    public const string Clients = nameof(Clients);
    public const string IdentityResources = nameof(IdentityResources);
    public const string ApiResources = nameof(ApiResources);
    public const string ApiScopes = nameof(ApiScopes);
    public const string UserSessions = nameof(UserSessions);
    public const string ServerSideSessions = nameof(ServerSideSessions);
    public const string IdentityProviders = nameof(IdentityProviders);
}

public static class EiaPermissions
{
    private static readonly EiaPermission[] AllPermissions =
    {
        new("View Dashboard", EiaAction.View, EiaResource.Dashboard),
        new("View Hangfire", EiaAction.View, EiaResource.Hangfire),
        new("View Users", EiaAction.View, EiaResource.Users),
        new("Search Users", EiaAction.Search, EiaResource.Users),
        new("Create Users", EiaAction.Create, EiaResource.Users),
        new("Update Users", EiaAction.Update, EiaResource.Users),
        new("Delete Users", EiaAction.Delete, EiaResource.Users),
        new("Export Users", EiaAction.Export, EiaResource.Users),
        new("View UserRoles", EiaAction.View, EiaResource.UserRoles),
        new("Update UserRoles", EiaAction.Update, EiaResource.UserRoles),
        new("View UserClaims", EiaAction.View, EiaResource.UserClaims),
        new("Search UserClaims", EiaAction.Search, EiaResource.UserClaims),
        new("Create UserClaims", EiaAction.Create, EiaResource.UserClaims),
        new("Update UserClaims", EiaAction.Update, EiaResource.UserClaims),
        new("Delete UserClaims", EiaAction.Delete, EiaResource.UserClaims),
        new("Search Roles", EiaAction.Search, EiaResource.Roles),
        new("View Roles", EiaAction.View, EiaResource.Roles),
        new("Create Roles", EiaAction.Create, EiaResource.Roles),
        new("Update Roles", EiaAction.Update, EiaResource.Roles),
        new("Delete Roles", EiaAction.Delete, EiaResource.Roles),
        new("View RoleClaims", EiaAction.View, EiaResource.RoleClaims),
        new("Search RoleClaims", EiaAction.Search, EiaResource.RoleClaims),
        new("Create RoleClaims", EiaAction.Create, EiaResource.RoleClaims),
        new("Update RoleClaims", EiaAction.Update, EiaResource.RoleClaims),
        new("Delete RoleClaims", EiaAction.Delete, EiaResource.RoleClaims),
        new("View Persisted Grants", EiaAction.View, EiaResource.PersistedGrants),
        new("Search Persisted Grants", EiaAction.Search, EiaResource.PersistedGrants),
        new("Create Persisted Grants", EiaAction.Create, EiaResource.PersistedGrants),
        new("Update Persisted Grants", EiaAction.Update, EiaResource.PersistedGrants),
        new("Delete Persisted Grants", EiaAction.Delete, EiaResource.PersistedGrants),
        new("Export Persisted Grants", EiaAction.Export, EiaResource.PersistedGrants),
        new("Search Audit Logs", EiaAction.Search, EiaResource.AuditLog),
        new("View Audit Logs", EiaAction.View, EiaResource.AuditLog),
        new("Search Clients", EiaAction.Search, EiaResource.Clients),
        new("View Clients", EiaAction.View, EiaResource.Clients),
        new("Create Clients", EiaAction.Create, EiaResource.Clients),
        new("Update Clients", EiaAction.Update, EiaResource.Clients),
        new("Delete Clients", EiaAction.Delete, EiaResource.Clients),
        new("Search IdentityResources", EiaAction.Search, EiaResource.IdentityResources),
        new("View IdentityResources", EiaAction.View, EiaResource.IdentityResources),
        new("Create IdentityResources", EiaAction.Create, EiaResource.IdentityResources),
        new("Update IdentityResources", EiaAction.Update, EiaResource.IdentityResources),
        new("Delete IdentityResources", EiaAction.Delete, EiaResource.IdentityResources),
        new("Search ApiScopes", EiaAction.Search, EiaResource.ApiResources),
        new("View ApiResources", EiaAction.View, EiaResource.ApiResources),
        new("Create ApiResources", EiaAction.Create, EiaResource.ApiResources),
        new("Update ApiResources", EiaAction.Update, EiaResource.ApiResources),
        new("Delete ApiResources", EiaAction.Delete, EiaResource.ApiResources),
        new("Search ApiResources", EiaAction.Search, EiaResource.ApiScopes),
        new("View ApiScopes", EiaAction.View, EiaResource.ApiScopes),
        new("Create ApiScopes", EiaAction.Create, EiaResource.ApiScopes),
        new("Update ApiScopes", EiaAction.Update, EiaResource.ApiScopes),
        new("Delete ApiScopes", EiaAction.Delete, EiaResource.ApiScopes),
        new("Search user sessions", EiaAction.Search, EiaResource.UserSessions),
        new("View user sessions", EiaAction.View, EiaResource.UserSessions),
        new("Create user sessions", EiaAction.Create, EiaResource.UserSessions),
        new("Update user sessions", EiaAction.Update, EiaResource.UserSessions),
        new("Delete user sessions", EiaAction.Delete, EiaResource.UserSessions),
        new("Search server-side sessions", EiaAction.Search, EiaResource.ServerSideSessions),
        new("View server-side sessions", EiaAction.View, EiaResource.ServerSideSessions),
        new("Create server-side sessions", EiaAction.Create, EiaResource.ServerSideSessions),
        new("Update server-side sessions", EiaAction.Update, EiaResource.ServerSideSessions),
        new("Delete server-side sessions", EiaAction.Delete, EiaResource.ServerSideSessions),
        new("Search identity providers", EiaAction.Search, EiaResource.IdentityProviders),
        new("View identity providers", EiaAction.View, EiaResource.IdentityProviders),
        new("Create identity provider", EiaAction.Create, EiaResource.IdentityProviders),
        new("Update identity provider", EiaAction.Update, EiaResource.IdentityProviders),
        new("Delete identity provider", EiaAction.Delete, EiaResource.IdentityProviders)
    };

    public static IReadOnlyList<EiaPermission> All { get; } = new ReadOnlyCollection<EiaPermission>(AllPermissions);

    public static IReadOnlyList<EiaPermission> Root { get; } = new ReadOnlyCollection<EiaPermission>(AllPermissions.Where(p => p.IsRoot).ToArray());

    public static IReadOnlyList<EiaPermission> Admin { get; } = new ReadOnlyCollection<EiaPermission>(AllPermissions.Where(p => !p.IsRoot).ToArray());

    public static IReadOnlyList<EiaPermission> Basic { get; } = new ReadOnlyCollection<EiaPermission>(AllPermissions.Where(p => p.IsBasic).ToArray());
}

public record EiaPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);

    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
