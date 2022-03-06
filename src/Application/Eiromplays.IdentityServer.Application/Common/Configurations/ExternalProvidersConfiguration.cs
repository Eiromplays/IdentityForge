namespace Eiromplays.IdentityServer.Application.Common.Configurations;

public class ExternalProvidersConfiguration
{
    public bool UseGoogleProvider { get; set; }
    public string GoogleClientId { get; set; } = "";
    public string GoogleClientSecret { get; set; } = "";
    public string GoogleCallbackPath { get; set; } = "";

    public bool UseGitHubProvider { get; set; }
    public string GitHubClientId { get; set; } = "";
    public string GitHubClientSecret { get; set; } = "";
    public string GitHubCallbackPath { get; set; } = "";

    public bool UseDiscordProvider { get; set; }
    public string DiscordClientId { get; set; } = "";
    public string DiscordClientSecret { get; set; } = "";
    public string DiscordCallbackPath { get; set; } = "";

    public bool UseRedditProvider { get; set; }
    public string RedditClientId { get; set; } = "";
    public string RedditClientSecret { get; set; } = "";
    public string RedditCallbackPath { get; set; } = "";

    public bool UseAmazonProvider { get; set; }
    public string AmazonClientId { get; set; } = "";
    public string AmazonClientSecret { get; set; } = "";
    public string AmazonCallbackPath { get; set; } = "";

    public bool UseTwitchProvider { get; set; }
    public string TwitchClientId { get; set; } = "";
    public string TwitchClientSecret { get; set; } = "";
    public string TwitchCallbackPath { get; set; } = "";

    public bool UsePatreonProvider { get; set; }
    public string PatreonClientId { get; set; } = "";
    public string PatreonClientSecret { get; set; } = "";
    public string PatreonCallbackPath { get; set; } = "";

    public bool UseWordpressProvider { get; set; }
    public string WordpressClientId { get; set; } = "";
    public string WordpressClientSecret { get; set; } = "";
    public string WordpressCallbackPath { get; set; } = "";
}