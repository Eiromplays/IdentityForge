using Eiromplays.IdentityServer.Domain.Enums;
using FluentEmail.Graph;
using FluentEmail.Mailgun;
using FluentEmail.MailKitSmtp;

namespace Eiromplays.IdentityServer.Application.Common.Configurations;

public class EmailConfiguration
{
    public EmailProvider EmailProvider { get; set; } = EmailProvider.MailKit;

    public string From { get; set; } = string.Empty;

    public string DefaultFromName { get; set; } = string.Empty;

    public SmtpEmailConfiguration? SmtpEmailConfiguration { get; set; }

    public SmtpClientOptions? MailKitConfiguration { get; set; }

    public SendGridConfiguration? SendGridConfiguration { get; set; }

    public MailgunConfiguration? MailgunConfiguration { get; set; }

    public MailtrapConfiguration? MailtrapConfiguration { get; set; }

    public GraphSenderOptions? GraphConfiguration { get; set; }
}

public class SmtpEmailConfiguration
{
    public string Host { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int Port { get; set; } = 587; // The default SMTP port

    public bool UseSsl { get; set; } = true;
}

public class SendGridConfiguration
{
    public string ApiKey { get; set; } = string.Empty;

    public bool SandboxMode { get; set; } = false;
}

public class MailgunConfiguration
{
    public string DomainName { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public MailGunRegion Region { get; set; } = MailGunRegion.EU;
}

public class MailtrapConfiguration
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }
}