using Eiromplays.IdentityServer.Application.Common.Mailing;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Mailing;

public class MailService : IMailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger<MailService> _logger;

    public MailService(ILogger<MailService> logger, IFluentEmail fluentEmail)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
    }

    public async Task SendAsync(MailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(request.Subject))
                _fluentEmail.Subject(request.Subject);

            if (!string.IsNullOrWhiteSpace(request.From))
                _fluentEmail.SetFrom(request.From, request.DisplayName);

            if (!string.IsNullOrWhiteSpace(request.ReplyTo))
                _fluentEmail.ReplyTo(request.ReplyTo);

            // To
            foreach (string to in request.To)
                _fluentEmail.To(to);

            // Bcc
            foreach (string address in request.Bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                _fluentEmail.BCC(address.Trim());

            // Cc
            foreach (string address in request.Cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                _fluentEmail.CC(address.Trim());

            // Headers
            foreach (var header in request.Headers)
                _fluentEmail.Header(header.Key, header.Value);

            _fluentEmail.Body(request.Body, true);

            // TODO: Implement file attachments
            // Create the file attachments for this e-mail message
            /*foreach (var attachmentInfo in request.AttachmentData)
                email.Attach(new Attachment { Filename = attachmentInfo.Key, Data = attachmentInfo.Value });*/

            await _fluentEmail.SendAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
        }
    }
}