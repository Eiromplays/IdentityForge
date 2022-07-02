using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Grants;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Grants;

namespace Eiromplays.IdentityServer.Endpoints.v1.Grants;

public class RevokeGrantEndpoint : Endpoint<RevokeGrantRequest, GrantResponse>
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;

    public RevokeGrantEndpoint(IIdentityServerInteractionService interaction, IEventService events)
    {
        _interaction = interaction;
        _events = events;
    }

    public override void Configure()
    {
        Delete("/grants/{ClientId}");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Revoke a grant";
        });
    }

    public override async Task HandleAsync(RevokeGrantRequest req, CancellationToken ct)
    {
        await _interaction.RevokeUserConsentAsync(req.ClientId);
        await _events.RaiseAsync(new GrantsRevokedEvent(User.GetSubjectId(), req.ClientId));

        await SendOkAsync(Response, ct);
    }
}