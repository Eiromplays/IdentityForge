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
        await SendOkAsync(await _consentService.GetConsentAsync(req), ct);
    }
}