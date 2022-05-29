namespace Shared.Authorization;

using System.Collections.ObjectModel;

public static class EIAAction
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

public static class EIAResource
{
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
    public const string PersistedGrants = nameof(PersistedGrants);
    public const string AuditLog = nameof(AuditLog);
}


public static class EIAPermissions
{
    private static readonly EIAPermission[] _all =
    {
        new("View Dashboard", EIAAction.View, EIAResource.Dashboard),
        new("View Hangfire", EIAAction.View, EIAResource.Hangfire),
        new("View Users", EIAAction.View, EIAResource.Users),
        new("Search Users", EIAAction.Search, EIAResource.Users),
        new("Create Users", EIAAction.Create, EIAResource.Users),
        new("Update Users", EIAAction.Update, EIAResource.Users),
        new("Delete Users", EIAAction.Delete, EIAResource.Users),
        new("Export Users", EIAAction.Export, EIAResource.Users),
        new("View UserRoles", EIAAction.View, EIAResource.UserRoles),
        new("Update UserRoles", EIAAction.Update, EIAResource.UserRoles),
        new("Search Roles", EIAAction.Search, EIAResource.Roles),
        new("View Roles", EIAAction.View, EIAResource.Roles),
        new("Create Roles", EIAAction.Create, EIAResource.Roles),
        new("Update Roles", EIAAction.Update, EIAResource.Roles),
        new("Delete Roles", EIAAction.Delete, EIAResource.Roles),
        new("View RoleClaims", EIAAction.View, EIAResource.RoleClaims),
        new("Update RoleClaims", EIAAction.Update, EIAResource.RoleClaims),
        new("View Products", EIAAction.View, EIAResource.Products, IsBasic: true),
        new("Search Products", EIAAction.Search, EIAResource.Products, IsBasic: true),
        new("Create Products", EIAAction.Create, EIAResource.Products),
        new("Update Products", EIAAction.Update, EIAResource.Products),
        new("Delete Products", EIAAction.Delete, EIAResource.Products),
        new("Export Products", EIAAction.Export, EIAResource.Products),
        new("View Brands", EIAAction.View, EIAResource.Brands, IsBasic: true),
        new("Search Brands", EIAAction.Search, EIAResource.Brands, IsBasic: true),
        new("Create Brands", EIAAction.Create, EIAResource.Brands),
        new("Update Brands", EIAAction.Update, EIAResource.Brands),
        new("Delete Brands", EIAAction.Delete, EIAResource.Brands),
        new("Generate Brands", EIAAction.Generate, EIAResource.Brands),
        new("Clean Brands", EIAAction.Clean, EIAResource.Brands),
        new("View Persisted Grants", EIAAction.View, EIAResource.PersistedGrants),
        new("Search Persisted Grants", EIAAction.Search, EIAResource.PersistedGrants),
        new("Create Persisted Grants", EIAAction.Create, EIAResource.PersistedGrants),
        new("Update Persisted Grants", EIAAction.Update, EIAResource.PersistedGrants),
        new("Delete Persisted Grants", EIAAction.Delete, EIAResource.PersistedGrants),
        new("Export Persisted Grants", EIAAction.Export, EIAResource.PersistedGrants),
        new("Search Audit Logs", EIAAction.Search, EIAResource.AuditLog),
        new("View Audit Logs", EIAAction.View, EIAResource.AuditLog),
    };

    public static IReadOnlyList<EIAPermission> All { get; } = new ReadOnlyCollection<EIAPermission>(_all);

    public static IReadOnlyList<EIAPermission> Root { get; } = new ReadOnlyCollection<EIAPermission>(_all.Where(p => p.IsRoot).ToArray());

    public static IReadOnlyList<EIAPermission> Admin { get; } = new ReadOnlyCollection<EIAPermission>(_all.Where(p => !p.IsRoot).ToArray());

    public static IReadOnlyList<EIAPermission> Basic { get; } = new ReadOnlyCollection<EIAPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record EIAPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);

    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
