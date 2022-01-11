namespace Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;

public class EmailConfiguration
{
    public string? From { get; set; }

    public string? Host { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int Port { get; set; } = 587; // The default SMTP port

    public bool UseSsl { get; set; } = true;
}