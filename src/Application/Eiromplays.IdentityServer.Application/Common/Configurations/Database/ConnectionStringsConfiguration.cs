namespace Eiromplays.IdentityServer.Application.Common.Configurations.Database;

[Serializable]
public class ConnectionStringsConfiguration
{
    public string ApplicationDbConnection { get; set; } = "";

    public string SessionDbConnection { get; set; } = "";

    public void SetConnections(string commonConnectionString)
    {
        ApplicationDbConnection = commonConnectionString;
        SessionDbConnection = commonConnectionString;
    }
}