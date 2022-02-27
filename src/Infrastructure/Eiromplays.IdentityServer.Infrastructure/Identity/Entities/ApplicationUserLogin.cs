using AutoMapper;
using Eiromplays.IdentityServer.Application.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

[AutoMap(typeof(UserLoginDto), ReverseMap = true)]
public class ApplicationUserLogin : IdentityUserLogin<string>
{

}