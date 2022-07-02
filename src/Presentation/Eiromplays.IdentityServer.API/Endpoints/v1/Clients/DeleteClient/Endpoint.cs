using Eiromplays.IdentityServer.Application.Identity.Clients;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Clients.DeleteClient;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IClientService _clientService;

    public Endpoint(IClientService clientService)
    {
        _clientService = clientService;
    }

    public override void Configure()
    {
        Delete("/clients/{Id:int}");
        Summary(s =>
        {
            s.Summary = "Delete a client.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Delete, EiaResource.Clients));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _clientService.DeleteAsync(req.Id, ct);

        await SendNoContentAsync(cancellation: ct);
    }
}