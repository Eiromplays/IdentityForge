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
        await SendOkAsync(await _consentService.ConsentAsync(req, User), ct);
    }
}