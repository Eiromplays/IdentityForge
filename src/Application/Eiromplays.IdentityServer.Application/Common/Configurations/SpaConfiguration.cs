namespace Eiromplays.IdentityServer.Application.Common.Configurations;

public class SpaConfiguration
{
    public string IdentityServerUiBaseUrl { get; set; } = "http://localhost:3000/";

    public string AdminUiBaseUrl { get; set; } = "http://admin.eiromplays.local.com/";
}