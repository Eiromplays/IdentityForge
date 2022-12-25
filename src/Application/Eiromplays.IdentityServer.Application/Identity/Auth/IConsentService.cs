using System.Security.Principal;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Consent;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Consent;

namespace Eiromplays.IdentityServer.Application.Identity.Auth;

public interface IConsentService : ITransientService
{
    Task<ConsentResponse> GetConsentAsync(GetConsentRequest request);

    Task<ProcessConsentResponse> ConsentAsync(ConsentRequest request, IPrincipal user);
}