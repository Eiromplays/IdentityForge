using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Grants;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Grants;

namespace Eiromplays.IdentityServer.Endpoints.v1.Grants;

public class GetGrantEndpoint : Endpoint<GetGrantRequest, GrantResponse>
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clients;
    private readonly IResourceStore _resources;

    public GetGrantEndpoint(IIdentityServerInteractionService interaction, IClientStore clients,
        IResourceStore resources)
    {
        _interaction = interaction;
        _clients = clients;
        _resources = resources;
    }

    public override void Configure()
    {
        Get("/grants/{ClientId}");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get a grant";
        });
    }
    
    public override async Task HandleAsync(GetGrantRequest req, CancellationToken ct)
    {
        var grants = await _interaction.GetAllUserGrantsAsync();

        var items = new List<GrantResponse>();
        foreach(var grant in grants)
        {
            var client = await _clients.FindClientByIdAsync(grant.ClientId);

            if (client is null) continue;

            var resources = await _resources.FindResourcesByScopeAsync(grant.Scopes);

            var item = new GrantResponse
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName ?? client.ClientId,
                ClientLogoUrl = client.LogoUri,
                ClientUrl = client.ClientUri,
                Description = grant.Description,
                Created = grant.CreationTime,
                Expires = grant.Expiration,
                IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                ApiGrantNames = resources.ApiScopes.Select(x => x.DisplayName ?? x.Name).ToArray()
            };
            
            items.Add(item);
        }

        var selectedGrant = items.FirstOrDefault(x => x.ClientId == req.ClientId);
        
        if (selectedGrant is null)
            throw new NotFoundException($"Grant with client id {req.ClientId} not found");

        await SendOkAsync(selectedGrant, ct);
    }
}