namespace Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;

public class ConnectionStringsConfiguration
{
    public string? IdentityDbConnection { get; set; }

    public string? ConfigurationDbConnection { get; set; }

    public string? PersistedGrantDbConnection { get; set; }

    public string? DataProtectionDbConnection { get; set; }

    public void SetConnections(string commonConnectionString)
    {
        IdentityDbConnection = commonConnectionString;
        ConfigurationDbConnection = commonConnectionString;
        DataProtectionDbConnection = commonConnectionString;
        PersistedGrantDbConnection = commonConnectionString;
    }
}