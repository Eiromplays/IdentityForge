namespace Eiromplays.IdentityServer.Application.Common.Configurations.Database;

[Serializable]
public class ConnectionStringsConfiguration
{
    public string IdentityDbConnection { get; set; } = "";

    public string ConfigurationDbConnection { get; set; } = "";

    public string PersistedGrantDbConnection { get; set; } = "";

    public string DataProtectionDbConnection { get; set; } = "";
    
    public string SessionDbConnection { get; set; } = "";

    public void SetConnections(string commonConnectionString)
    {
        IdentityDbConnection = commonConnectionString;
        ConfigurationDbConnection = commonConnectionString;
        DataProtectionDbConnection = commonConnectionString;
        PersistedGrantDbConnection = commonConnectionString;
        SessionDbConnection = commonConnectionString;
    }
}