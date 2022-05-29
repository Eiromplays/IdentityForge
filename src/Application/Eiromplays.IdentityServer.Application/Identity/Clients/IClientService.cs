namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public interface IClientService : ITransientService
{
    Task<PaginationResponse<Duende.IdentityServer.Models.Client>> SearchAsync(ClientListFilter filter,
        CancellationToken cancellationToken = default);
    
    Task<Duende.IdentityServer.Models.Client> GetAsync(string? clientId, CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdateClientRequest request, string clientId, CancellationToken cancellationToken = default);
}