using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Consent;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Consent;

namespace Eiromplays.IdentityServer.Endpoints.v1.Consent;

public class GetConsentEndpoint : Endpoint<GetConsentRequest, ConsentResponse>
{
    private readonly IConsentService _consentService;

    public GetConsentEndpoint(IConsentService consentService)
    {
        _consentService = consentService;
    }

    public override void Configure()
    {
        Get("/consent");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get Consent";
        });
    }
    
    public override async Task HandleAsync(GetConsentRequest req, CancellationToken ct)
    {
        var result = await _consentService.GetConsentAsync(req);
        
        await result.Match(async x =>
        {
            await SendOkAsync(x, cancellation: ct);
        }, async exception =>
        {
            switch (exception)
            {
                case BadRequestException badRequestException:
                    ThrowError(badRequestException.Message);
                    return;
                case InternalServerException internalServerException:
                    AddError(internalServerException.Message);
                    await SendErrorsAsync((int)internalServerException.StatusCode, ct);
                    return;
            }

            await SendErrorsAsync(cancellation: ct);
        });
    }
}