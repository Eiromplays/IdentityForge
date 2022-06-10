using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.ExternalLogins;

namespace Eiromplays.IdentityServer.Endpoints.v1.ExternalLogins;

public class ExternalLoginEndpoint : Endpoint<ExternalLoginRequest, object>
{
    private readonly IAuthService _authService;

    public ExternalLoginEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Verbs(Http.GET, Http.POST);
        Routes("/external-logins");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get or create an external login";
        });
    }
    
    public override async Task HandleAsync(ExternalLoginRequest req, CancellationToken ct)
    {
        var result = _authService.ExternalLogin(req, HttpContext);
        
        await result.Match(async x =>
        {
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