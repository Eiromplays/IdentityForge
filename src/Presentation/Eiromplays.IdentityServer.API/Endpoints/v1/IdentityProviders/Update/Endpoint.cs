using Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityProviders.Update;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IIdentityProviderService _identityProviderService;

    public Endpoint(IIdentityProviderService identityProviderService)
    {
        _identityProviderService = identityProviderService;
    }

    public override void Configure()
    {
        Put("/identity-providers/{Id}");
        Summary(s =>
        {
            s.Summary = "Update a identity provider";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Update, EiaResource.IdentityProviders));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _identityProviderService.UpdateAsync(req.Data, req.Id, ct);

        await SendNoContentAsync(cancellation: ct);
    }
}