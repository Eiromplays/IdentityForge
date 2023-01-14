using Eiromplays.IdentityServer.Application.Identity.IdentityProviders;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityProviders.Create;

public class Endpoint : Endpoint<CreateIdentityProviderRequest, Models.Response>
{
    private readonly IIdentityProviderService _identityProviderService;

    public Endpoint(IIdentityProviderService identityProviderService)
    {
        _identityProviderService = identityProviderService;
    }

    public override void Configure()
    {
        Post("/identity-providers");
        Summary(s =>
        {
            s.Summary = "Creates a new identity provider.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Create, EiaResource.IdentityProviders));
    }

    public override async Task HandleAsync(CreateIdentityProviderRequest req, CancellationToken ct)
    {
        Response.Message = await _identityProviderService.CreateAsync(req, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}