namespace IdentityForge.IdentityServer.Configurations;

public class ConnectionStringOptions
{
    public const string SectionName = "ConnectionStrings";
    public const string IdentityForgeKey = nameof(IdentityForge);

    public string IdentityForge { get; set; } = string.Empty;
}

public static class ConnectionStringOptionsExtensions
{
    public static ConnectionStringOptions GetConnectionStringOptions(this IConfiguration configuration)
        => configuration.GetSection(ConnectionStringOptions.SectionName).Get<ConnectionStringOptions>() ?? new ConnectionStringOptions();
}