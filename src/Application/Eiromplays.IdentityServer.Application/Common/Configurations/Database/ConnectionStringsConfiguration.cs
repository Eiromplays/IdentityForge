namespace Eiromplays.IdentityServer.Application.Common.Configurations.Database;

[Serializable]
public class ConnectionStringsConfiguration
{
    public string ApplicationDbConnection { get; set; } = string.Empty;

    public string SessionDbConnection { get; set; } = string.Empty;

    public void SetConnections(string commonConnectionString)
    {
        ApplicationDbConnection = commonConnectionString;
        SessionDbConnection = commonConnectionString;
    }
}