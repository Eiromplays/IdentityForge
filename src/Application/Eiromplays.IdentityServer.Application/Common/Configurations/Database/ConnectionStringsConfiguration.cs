namespace Eiromplays.IdentityServer.Application.Common.Configurations.Database;

[Serializable]
public class ConnectionStringsConfiguration
{
    public string ApplicationDbConnection { get; set; } = "";

    public string ConfigurationDbConnection { get; set; } = "";

    public string PersistedGrantDbConnection { get; set; } = "";

    public string DataProtectionDbConnection { get; set; } = "";

    public string SessionDbConnection { get; set; } = "";
    
    public string TenantDbConnection { get; set; } = "";

    public void SetConnections(string commonConnectionString)
    {
        ApplicationDbConnection = commonConnectionString;
        ConfigurationDbConnection = commonConnectionString;
        DataProtectionDbConnection = commonConnectionString;
        PersistedGrantDbConnection = commonConnectionString;
        SessionDbConnection = commonConnectionString;
        TenantDbConnection = commonConnectionString;
    }
}