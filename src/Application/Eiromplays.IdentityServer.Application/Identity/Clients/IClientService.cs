namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public interface IClientService : ITransientService
{
    Task<ClientDto> GetAsync(string? clientId, CancellationToken cancellationToken = default);
}