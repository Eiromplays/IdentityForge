using Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityProviders.GetById;

public class Endpoint : Endpoint<Models.Request, IdentityProviderDto>
{
    private readonly IIdentityProviderService _identityProviderService;

    public Endpoint(IIdentityProviderService identityProviderService)
    {
        _identityProviderService = identityProviderService;
    }

    public override void Configure()
    {
        Get("/identity-providers/{Id}");
        Summary(s =>
        {
            s.Summary = "Get identity provider details.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.IdentityProviders));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _identityProviderService.GetAsync(request.Id, ct);

        await SendAsync(Response, cancellation: ct);
    }
}