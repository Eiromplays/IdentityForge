using Eiromplays.IdentityServer.Application.Common.Sms;
using FluentSMS.Core;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Sms;

public class SmsService : ISmsService
{
    private readonly IFluentSms _fluentSms;
    private readonly ILogger<SmsService> _logger;

    public SmsService(ILogger<SmsService> logger, IFluentSms fluentSms)
    {
        _fluentSms = fluentSms;
        _logger = logger;
    }

    public async Task SendAsync(SmsRequest request, CancellationToken ct)
    {
        try
        {
            _fluentSms.To(request.To);

            _fluentSms.Body(request.Body);

            await _fluentSms.SendAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
        }
    }
}