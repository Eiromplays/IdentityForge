namespace Eiromplays.IdentityServer.Application.Common.Caching;

public class CloudflareConfiguration
{
    public bool Enabled { get; set; } = false;
    public string ApiToken { get; set; } = default!;

    public string AccountId { get; set; } = default!;

    public string ApiBaseUrl { get; set; } = default!;

    public string ImagesBaseUrl { get; set; } = default!;
}