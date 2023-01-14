using Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityProviders.Delete;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IIdentityProviderService _identityProviderService;

    public Endpoint(IIdentityProviderService identityProviderService)
    {
        _identityProviderService = identityProviderService;
    }

    public override void Configure()
    {
        Delete("/identity-providers/{Id:int}");
        Summary(s =>
        {
            s.Summary = "Delete a identity provider.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.IdentityProviders));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _identityProviderService.DeleteAsync(req.Id, ct);

        await SendNoContentAsync(cancellation: ct);
    }
}