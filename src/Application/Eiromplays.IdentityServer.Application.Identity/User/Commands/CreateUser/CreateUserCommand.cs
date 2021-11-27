using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.DTOs.Role;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using MediatR;

namespace Eiromplays.IdentityServer.Application.Identity.User.Commands.CreateUser;

public class CreateUserCommand<TUserDto, TKey> : IRequest<(Result Result, string? UserId)>
    where TUserDto : UserDto<TKey>
{
    public TUserDto? UserDto { get; set; }
}

public class CreateUserCommandHandler<TUserDto, TRoleDto, TKey> : IRequestHandler<CreateUserCommand<TUserDto, TKey>, (Result Result, string? UserId)>
    where TUserDto : UserDto<TKey>
    where TRoleDto : RoleDto<TKey>
{
    private readonly IIdentityService<TUserDto, TRoleDto> _identityService;

    public CreateUserCommandHandler(IIdentityService<TUserDto, TRoleDto> identityService)
    {
        _identityService = identityService;
    }

    public async Task<(Result Result, string? UserId)> Handle(CreateUserCommand<TUserDto, TKey> request, CancellationToken cancellationToken)
    {
        return await _identityService.CreateUserAsync(request.UserDto);
    }
}