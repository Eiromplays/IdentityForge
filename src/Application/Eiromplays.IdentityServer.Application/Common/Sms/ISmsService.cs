namespace Eiromplays.IdentityServer.Application.Common.Sms;

public interface ISmsService : ITransientService
{
    Task SendAsync(SmsRequest request, CancellationToken ct);
}