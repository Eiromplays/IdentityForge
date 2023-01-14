namespace Eiromplays.IdentityServer.Application.Common.Configurations;

public class UrlConfiguration
{
    public string IdentityServerBaseUrl { get; set; } = "https://localhost:7001/";

    public string ApiBaseUrl { get; set; } = "https://localhost:7003/";

    public string IdentityServerUiBaseUrl { get; set; } = "https://localhost:3000/";

    public string AdminUiBaseUrl { get; set; } = "https://localhost:3001/";
}