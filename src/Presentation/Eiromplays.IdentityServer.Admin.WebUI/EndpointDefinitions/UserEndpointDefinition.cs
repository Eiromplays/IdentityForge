using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Eiromplays.IdentityServer.Infrastructure.Identity.Services;

namespace Eiromplays.IdentityServer.Admin.EndpointDefinitions;

public class UserEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/users/{search}/{pageIndex:int?}/{pageSize:int?}", GetAllUsersAsync);
        app.MapGet("/users/{id}", GetUserByIdAsync);
        app.MapPost("/users", CreateUserAsync);
        app.MapPut("/users/{id}", UpdateUserAsync);
        app.MapDelete("/users/{id}", DeleteUserByIdAsync);
    }

    internal async Task<PaginatedList<UserDto>> GetAllUsersAsync(IdentityService identityService, string? search,
        int pageIndex = 1, int pageSize = 10)
    {
        return await identityService.GetUsersAsync(search, pageIndex, pageSize);
    }

    internal async Task<UserDto?> GetUserByIdAsync(IdentityService identityService, string id)
    {
        return await identityService.FindUserByIdAsync(id);
    }

    internal async Task<(Result Result, string? UserId)> CreateUserAsync(IdentityService identityService, UserDto userDto)
    {
        return await identityService.CreateUserAsync(userDto);
    }

    internal async Task<(Result Result, string? UserId)> UpdateUserAsync(IdentityService identityService, UserDto userDto)
    {
        return await identityService.UpdateUserAsync(userDto);
    }

    internal async Task<Result> DeleteUserByIdAsync(IdentityService identityService, string id)
    {
        return await identityService.DeleteUserAsync(id);
    }

    public void DefineServices(IServiceCollection services) { }
}