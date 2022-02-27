using AutoMapper;
using Eiromplays.IdentityServer.Application.DTOs.Role;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

[AutoMap(typeof(RoleDto), ReverseMap = true)]
public class ApplicationRole : IdentityRole
{
}