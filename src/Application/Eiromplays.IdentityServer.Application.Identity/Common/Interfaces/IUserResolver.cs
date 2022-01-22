namespace Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;

public interface IUserResolver<TUser>
    where TUser : class
{
    Task<TUser?> GetUserAsync(string? identifier);
}