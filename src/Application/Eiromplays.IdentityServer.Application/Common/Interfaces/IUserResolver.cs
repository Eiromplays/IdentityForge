namespace Eiromplays.IdentityServer.Application.Common.Interfaces;

public interface IUserResolver<TUser>
    where TUser : class
{
    Task<TUser?> GetUserAsync(string? identifier);
}