using FluentValidation;

namespace Eiromplays.IdentityServer.Application.Permissions.Queries.GetPermissionsWithPagination
{
    public class GetPermissionsWithPaginationQueryValidator<TPermissionDto, TKey> : AbstractValidator<GetPermissionsWithPaginationQuery<TPermissionDto, TKey>>
        where TPermissionDto : PermissionDto<TKey>
    {
        public GetPermissionsWithPaginationQueryValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }
}