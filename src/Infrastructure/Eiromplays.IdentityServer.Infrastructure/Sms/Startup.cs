using Eiromplays.IdentityServer.Application.Common.Configurations;
using Eiromplays.IdentityServer.Domain.Enums;
using FluentSms.Core;
using FluentSms.Infobip;
using FluentSms.Twilio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eiromplays.IdentityServer.Infrastructure.Sms;

internal static class Startup
{
    internal static IServiceCollection AddSms(this IServiceCollection services, IConfiguration configuration)
    {
        var smsConfiguration = configuration.GetSection(nameof(SmsConfiguration)).Get<SmsConfiguration>();
        var fluentSmsServicesBuilder = services
            .AddFluentSms(smsConfiguration.From);

        switch (smsConfiguration.SmsProvider)
        {
            case SmsProvider.Infobip:
                return smsConfiguration.InfobipConfiguration is null
                    ? services
                    : fluentSmsServicesBuilder.AddInfobipSender(
                        smsConfiguration.InfobipConfiguration.ApiKey,
                        smsConfiguration.InfobipConfiguration.ApiKeyPrefix,
                        smsConfiguration.InfobipConfiguration.BasePath).Services;
            case SmsProvider.Twilio:
                return smsConfiguration.TwilioConfiguration is null
                    ? services
                    : fluentSmsServicesBuilder.AddTwilioSender(
                        smsConfiguration.TwilioConfiguration.AccountSid,
                        smsConfiguration.TwilioConfiguration.AuthToken).Services;
            default:
                const string nameOfSmsProvider = nameof(smsConfiguration.SmsProvider);
                throw new ArgumentOutOfRangeException(
                    nameOfSmsProvider,
                    $"SmsProvider needs to be one of these: {string.Join(", ", Enum.GetNames(typeof(SmsProvider)))}.");
        }
    }
}