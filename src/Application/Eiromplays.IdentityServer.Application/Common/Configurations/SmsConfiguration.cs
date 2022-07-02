using Eiromplays.IdentityServer.Domain.Enums;

namespace Eiromplays.IdentityServer.Application.Common.Configurations;

public class SmsConfiguration
{
    public SmsProvider SmsProvider { get; set; }

    public string From { get; set; } = string.Empty;

    public TwilioConfiguration? TwilioConfiguration { get; set; }

    public InfobipConfiguration? InfobipConfiguration { get; set; }
}

public class InfobipConfiguration
{
    public string ApiKey { get; set; } = string.Empty;

    public string ApiKeyPrefix { get; set; } = "App";

    public string BasePath { get; set; } = string.Empty;
}

public class TwilioConfiguration
{
    public string AccountSid { get; set; } = string.Empty;

    public string AuthToken { get; set; } = string.Empty;
}