using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Consent;

namespace Eiromplays.IdentityServer.Endpoints.v1.Consent;

public class ConsentEndpoint : Endpoint<ConsentResponse, ProcessConsentResponse>
{
    private readonly IConsentService _consentService;

    public ConsentEndpoint(IConsentService consentService)
    {
        _consentService = consentService;
    }

    public override void Configure()
    {
        Post("/consent");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Consent";
        });
    }
    
    public override async Task HandleAsync(ConsentResponse req, CancellationToken ct)
    {
        var result = await _consentService.ConsentAsync(req, User);
        
        await result.Match(async x =>
        {
            await SendOkAsync(x, cancellation: ct);
        }, async exception =>
        {
            switch (exception)
            {
                case BadRequestException badRequestException:
                    ThrowError(badRequestException.Message);
                    break;
                case InternalServerException internalServerException:
                    AddError(internalServerException.Message);
                    await SendErrorsAsync((int)internalServerException.StatusCode, ct);
                    break;
            }

            await SendErrorsAsync(cancellation: ct);
        });
    }
}