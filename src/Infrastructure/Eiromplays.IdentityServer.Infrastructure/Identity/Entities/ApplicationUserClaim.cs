using AutoMapper;
using Eiromplays.IdentityServer.Application.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

[AutoMap(typeof(UserClaimDto), ReverseMap = true)]
public class ApplicationUserClaim : IdentityUserClaim<string>
{
}