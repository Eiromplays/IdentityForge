using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;

namespace Eiromplays.IdentityServer.Endpoints.v1.ExternalLogins;

public class GetExternalLoginCallbackEndpoint : Endpoint<GetExternalLoginCallbackRequest>
{
    private readonly IAuthService _authService;

    public GetExternalLoginCallbackEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Get("/external-logins/callback");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get error information";
        });
    }
    
    public override async Task HandleAsync(GetExternalLoginCallbackRequest req, CancellationToken ct)
    {
        var result =  await _authService.ExternalLoginCallbackAsync(req);
        
        await result.Match(async x =>
        {
            if (!string.IsNullOrWhiteSpace(x.ExternalLoginReturnUrl))
                await SendRedirectAsync(x.ExternalLoginReturnUrl, true, ct);
            
            await SendOkAsync(x, cancellation: ct);
        }, exception =>
        {
            if (exception is BadRequestException badRequestException)
            {
                ThrowError(badRequestException.Message);
            }

            return SendErrorsAsync(cancellation: ct);
        });
    }
}