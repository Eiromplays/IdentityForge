namespace Eiromplays.IdentityServer.Application.Common.Sms;

public class SmsRequest
{
    public SmsRequest(List<string> to, string body, string? from = null)
    {
        Body = body;
        From = from;
        To = to;
    }

    public List<string> To { get; }

    public string Body { get; }

    public string? From { get; }
}