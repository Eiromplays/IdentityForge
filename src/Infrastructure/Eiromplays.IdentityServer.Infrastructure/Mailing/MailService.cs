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
            var email = _fluentEmail.Subject(request.Subject).ReplyTo(request.ReplyTo)
                .SetFrom(request.From, request.DisplayName);
            
            // To
            foreach (var to in request.To)
                email.To(to);

            // Bcc
            foreach (var address in request.Bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                email.BCC(address.Trim());

            // Cc
            foreach (var address in request.Cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                email.CC(address.Trim());

            // Headers
            foreach (var header in request.Headers)
                email.Header(header.Key, header.Value);

            //TODO: Implement file attachments
            // Create the file attachments for this e-mail message
            /*foreach (var attachmentInfo in request.AttachmentData)
                email.Attach(new Attachment { Filename = attachmentInfo.Key, Data = attachmentInfo.Value });*/

            await _fluentEmail.SendAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}