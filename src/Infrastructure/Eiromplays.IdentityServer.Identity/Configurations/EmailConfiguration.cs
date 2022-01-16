using Eiromplays.IdentityServer.Domain.Enums;
using FluentEmail.Graph;
using FluentEmail.Mailgun;
using FluentEmail.MailKitSmtp;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Configurations;

public class EmailConfiguration
{
    public EmailProvider EmailProvider { get; set; }

    public string? From { get; set; }

    public string? DefaultFromName { get; set; }

    public SmtpEmailConfiguration? SmtpEmailConfiguration { get; set; }

    public SmtpClientOptions? MailKitConfiguration { get; set; }

    public SendGridConfiguration? SendGridConfiguration { get; set; }

    public MailgunConfiguration? MailgunConfiguration { get; set; }

    public MailtrapConfiguration? MailtrapConfiguration { get; set; }

    public GraphSenderOptions? GraphConfiguration { get; set; }
}

public class SmtpEmailConfiguration
{
    public string? Host { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int Port { get; set; } = 587; // The default SMTP port

    public bool UseSsl { get; set; } = true;
}

public class SendGridConfiguration
{
    public string? ApiKey { get; set; }

    public bool SandboxMode { get; set; } = false;
}

public class MailgunConfiguration
{
    public string? DomainName { get; set; }

    public string? ApiKey { get; set; }

    public MailGunRegion Region { get; set; }
}

public class MailtrapConfiguration
{
    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Host { get; set; }

    public int Port { get; set; }
}