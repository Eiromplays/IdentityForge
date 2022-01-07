using AutoMapper;
using Eiromplays.IdentityServer.Application.Identity.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

[AutoMap(typeof(UserDto), ReverseMap = true)]
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string? DisplayName { get; set; }

    [ProtectedPersonalData]
    public override string? Email { get; set; }

    [PersonalData]
    public string? ProfilePicture { get; set; }

    [PersonalData]
    public string? GravatarEmail { get; set; }

    [PersonalData]
    public decimal Credits { get; set; }

    [PersonalData]
    public string? DiscordId { get; set; }
}