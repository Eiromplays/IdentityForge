using AutoMapper;
using Eiromplays.IdentityServer.Application.DTOs.User;
using Eiromplays.IdentityServer.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

[AutoMap(typeof(UserDto), ReverseMap = true)]
public class ApplicationUser : IdentityUser, IAuditableEntity
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
    public double Credits { get; set; }

    [PersonalData]
    public string? DiscordId { get; set; }
    
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}