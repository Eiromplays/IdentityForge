namespace Eiromplays.IdentityServer.Application.Common.Configurations.Account;

public class ExternalProvidersConfiguration
{
    public bool UseGoogleProvider { get; set; }
    public string GoogleClientId { get; set; } = string.Empty;
    public string GoogleClientSecret { get; set; } = string.Empty;
    public string GoogleCallbackPath { get; set; } = string.Empty;

    public bool UseGitHubProvider { get; set; }
    public string GitHubClientId { get; set; } = string.Empty;
    public string GitHubClientSecret { get; set; } = string.Empty;
    public string GitHubCallbackPath { get; set; } = string.Empty;

    public bool UseDiscordProvider { get; set; }
    public string DiscordClientId { get; set; } = string.Empty;
    public string DiscordClientSecret { get; set; } = string.Empty;
    public string DiscordCallbackPath { get; set; } = string.Empty;

    public bool UseRedditProvider { get; set; }
    public string RedditClientId { get; set; } = string.Empty;
    public string RedditClientSecret { get; set; } = string.Empty;
    public string RedditCallbackPath { get; set; } = string.Empty;

    public bool UseAmazonProvider { get; set; }
    public string AmazonClientId { get; set; } = string.Empty;
    public string AmazonClientSecret { get; set; } = string.Empty;
    public string AmazonCallbackPath { get; set; } = string.Empty;

    public bool UseTwitchProvider { get; set; }
    public string TwitchClientId { get; set; } = string.Empty;
    public string TwitchClientSecret { get; set; } = string.Empty;
    public string TwitchCallbackPath { get; set; } = string.Empty;

    public bool UsePatreonProvider { get; set; }
    public string PatreonClientId { get; set; } = string.Empty;
    public string PatreonClientSecret { get; set; } = string.Empty;
    public string PatreonCallbackPath { get; set; } = string.Empty;

    public bool UseWordpressProvider { get; set; }
    public string WordpressClientId { get; set; } = string.Empty;
    public string WordpressClientSecret { get; set; } = string.Empty;
    public string WordpressCallbackPath { get; set; } = string.Empty;
}