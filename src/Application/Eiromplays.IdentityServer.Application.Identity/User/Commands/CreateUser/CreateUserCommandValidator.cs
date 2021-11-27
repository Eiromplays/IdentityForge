using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using FluentValidation;

namespace Eiromplays.IdentityServer.Application.Identity.User.Commands.CreateUser;

public class CreateUserCommandValidator<TUserDto, TKey> : AbstractValidator<CreateUserCommand<TUserDto>>
    where TUserDto : UserDto<TKey>
    where TKey : IEquatable<TKey>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserDto!.UserName).NotEmpty().WithMessage("Required Field")
            .Matches(@"^[a-zA-Z0-9_@\-\.\\\s*]+$");

        RuleFor(x => x.UserDto!.DisplayName)
            .Matches(@"^[a-zA-Z0-9_@\-\.\\\s*]+$");

        RuleFor(x => x.UserDto!.Email).NotEmpty().WithMessage("Required Field")
            .EmailAddress();

        RuleFor(x => x.UserDto!.GravatarEmail)
            .EmailAddress();
    }
}