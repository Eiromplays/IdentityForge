namespace Eiromplays.IdentityServer.Application.Common.Interfaces;

public interface IUserResolver<TUser> : ITransientService
    where TUser : class
{
    Task<TUser?> GetUserAsync(string? identifier, CancellationToken ct = default);
}