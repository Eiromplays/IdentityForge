using System.Net;
using System.Net.Mail;
using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Domain.Enums;
using FluentEmail.Graph;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eiromplays.IdentityServer.Infrastructure.Mailing;

internal static class Startup
{
    internal static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration configuration)
    {
        var emailConfiguration = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
        var fluentEmailServicesBuilder =services
            .AddFluentEmail(emailConfiguration.From, emailConfiguration.DefaultFromName)
            .AddRazorRenderer();
        
        switch (emailConfiguration.EmailProvider)
        {
            case EmailProvider.Smtp:
                return emailConfiguration.SmtpEmailConfiguration is null
                    ? services
                    : fluentEmailServicesBuilder.AddSmtpSender(
                        new SmtpClient(emailConfiguration.SmtpEmailConfiguration.Host,
                            emailConfiguration.SmtpEmailConfiguration.Port)
                        {
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(emailConfiguration.SmtpEmailConfiguration.Login,
                                emailConfiguration.SmtpEmailConfiguration.Password),
                            EnableSsl = emailConfiguration.SmtpEmailConfiguration.UseSsl
                        }).Services;
            case EmailProvider.MailKit:
                return emailConfiguration.MailKitConfiguration is null
                    ? services
                    : fluentEmailServicesBuilder.AddMailKitSender(emailConfiguration.MailKitConfiguration).Services;
            case EmailProvider.SendGrid:
                return emailConfiguration.SendGridConfiguration is null
                    ? services
                    : fluentEmailServicesBuilder.AddSendGridSender(emailConfiguration.SendGridConfiguration.ApiKey,
                        emailConfiguration.SendGridConfiguration.SandboxMode).Services;
            case EmailProvider.Mailgun:
                return emailConfiguration.MailgunConfiguration is null
                    ? services
                    : fluentEmailServicesBuilder.AddMailGunSender(emailConfiguration.MailgunConfiguration.DomainName,
                            emailConfiguration.MailgunConfiguration.ApiKey,
                            emailConfiguration.MailgunConfiguration.Region)
                        .Services;
            case EmailProvider.Mailtrap:
                return emailConfiguration.MailtrapConfiguration is null
                    ? services
                    : fluentEmailServicesBuilder.AddMailtrapSender(emailConfiguration.MailtrapConfiguration.UserName,
                        emailConfiguration.MailtrapConfiguration.Password,
                        emailConfiguration.MailtrapConfiguration.Host,
                        emailConfiguration.MailtrapConfiguration.Port).Services;
            case EmailProvider.Graph:
                return emailConfiguration.GraphConfiguration is null
                    ? services
                    : fluentEmailServicesBuilder.AddGraphSender(emailConfiguration.GraphConfiguration).Services;
            default:
                const string nameOfEmailProvider = nameof(emailConfiguration.EmailProvider);
                throw new ArgumentOutOfRangeException(nameOfEmailProvider,
                    $"EmailProvider needs to be one of these: {string.Join(", ", Enum.GetNames(typeof(EmailProvider)))}.");
        }
    }
}