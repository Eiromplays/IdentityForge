using Microsoft.AspNetCore.Identity;

namespace IdentityForge.IdentityServer.Domain.Users;

public class ApplicationUser : IdentityUser, IBaseEntity
{
    [ProtectedPersonalData]
    public required string FirstName { get; set; }

    [ProtectedPersonalData]
    public required string MiddleName { get; set; }

    [ProtectedPersonalData]
    public required string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    [PersonalData]
    public string? DisplayName { get; set; }

    [PersonalData]
    public string? ProfilePicture { get; set; }

    [ProtectedPersonalData]
    public string? GravatarEmail { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public required string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public required string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}