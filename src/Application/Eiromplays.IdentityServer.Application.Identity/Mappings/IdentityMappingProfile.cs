using AutoMapper;
using Eiromplays.IdentityServer.Application.Identity.DTOs.Role;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;

namespace Eiromplays.IdentityServer.Application.Identity.Mappings;

public class IdentityMappingProfile<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserLogin, TRoleClaim, TUserLoginDto, TUserClaimDto, TRoleClaimDto> : Profile
    where TUserDto : UserDto<TKey>
    where TRoleDto : RoleDto<TKey>
    where TUserLoginDto : UserLoginDto<TKey>
    where TUserClaimDto : UserClaimDto<TKey>
    where TRoleClaimDto : RoleClaimDto<TKey>
{
    public IdentityMappingProfile()
    {
        // Create mappings from entities to DTOs and backwards

        CreateMap<TUser, TUserDto>().ReverseMap();

        CreateMap<TRole, TRoleDto>().ReverseMap();

        CreateMap<TUserClaim, TUserClaimDto>().ReverseMap();

        CreateMap<TUserLogin, TUserLoginDto>().ReverseMap();

        CreateMap<TRoleClaim, TRoleClaimDto>().ReverseMap();
    }
}