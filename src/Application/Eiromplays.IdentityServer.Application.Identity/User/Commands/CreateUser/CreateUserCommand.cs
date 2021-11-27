using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Application.Common.Models;
using MediatR;

namespace Eiromplays.IdentityServer.Application.Identity.User.Commands.CreateUser;

public class CreateUserCommand<TUserDto> : IRequest<(Result Result, string? UserId)>
    where TUserDto : class
{
    public TUserDto? UserDto { get; set; }
}

public class CreateUserCommandHandler<TUserDto, TRoleDto> : IRequestHandler<CreateUserCommand<TUserDto>, (Result Result, string? UserId)>
    where TUserDto : class
    where TRoleDto : class
{
    private readonly IIdentityService<TUserDto, TRoleDto> _identityService;

    public CreateUserCommandHandler(IIdentityService<TUserDto, TRoleDto> identityService)
    {
        _identityService = identityService;
    }

    public async Task<(Result Result, string? UserId)> Handle(CreateUserCommand<TUserDto> request, CancellationToken cancellationToken)
    {
        return await _identityService.CreateUserAsync(request.UserDto);
    }
}