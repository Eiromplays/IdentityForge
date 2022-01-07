using Eiromplays.IdentityServer.Application.Identity.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;

namespace Eiromplays.IdentityServer.Auth.Backend.EndpointDefinitions;

public class UserEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/users/{search}/{pageIndex?}/{pageSize?}",
                async (string search, int? pageIndex, int? pageSize, IIdentityService identityService) =>
                    await GetAllUsersAsync(identityService, search, pageIndex ?? 1, pageSize ?? 10));
        app.MapGet("/users/{id}", GetUserByIdAsync);
        app.MapPost("/users", CreateUserAsync);
        app.MapPut("/users/{id}", UpdateUserAsync);
        app.MapDelete("/users/{id}", DeleteUserByIdAsync);
    }

    internal async Task<IResult> GetAllUsersAsync(IIdentityService identityService, string? search,
        int pageIndex = 1, int pageSize = 10)
    {
        var users = await identityService.GetUsersAsync(search, pageIndex, pageSize);

        return Results.Ok(users);
    }

    internal async Task<IResult> GetUserByIdAsync(IIdentityService identityService, string id)
    {
        return Results.Ok(await identityService.FindUserByIdAsync(id));
    }

    internal async Task<IResult> CreateUserAsync(IIdentityService identityService, UserDto userDto)
    {
        var (result, userId) = await identityService.CreateUserAsync(userDto);

        return result.Succeeded ? Results.Created($"users/{userId}", new { UserId = userId }) : Results.BadRequest(result);
    }

    internal async Task<IResult> UpdateUserAsync(IIdentityService identityService, UserDto userDto)
    {
        return Results.Ok(await identityService.UpdateUserAsync(userDto));
    }

    internal async Task<IResult> DeleteUserByIdAsync(IIdentityService identityService, string id)
    {
        return Results.Ok(await identityService.DeleteUserAsync(id));
    }

    public void DefineServices(IServiceCollection services)
    {

    }
}