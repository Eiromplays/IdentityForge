using Eiromplays.IdentityServer.Application.Common.Models;

namespace Eiromplays.IdentityServer.Application.Common.Interface;

public interface IIdentityService<in TUserDto, TRoleDto>
    where TUserDto : class
    where TRoleDto : class
{
    Task<(Result Result, string? UserId)> CreateUserAsync(TUserDto? userDto);

    Task<string?> GetUserNameAsync(string userId);
}