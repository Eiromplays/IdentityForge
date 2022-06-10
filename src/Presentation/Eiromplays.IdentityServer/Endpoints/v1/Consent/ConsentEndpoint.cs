using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Consent;

namespace Eiromplays.IdentityServer.Endpoints.v1.Consent;

public class ConsentEndpoint : Endpoint<ConsentRequest>
{
    private readonly IAuthService _authService;

    public ConsentEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/consent");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get error information";
        });
    }
    
    public override async Task HandleAsync(ConsentRequest req, CancellationToken ct)
    {
        var result = await _authService.ConsentAsync(req);
        
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