using Eiromplays.IdentityServer.Domain.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

public class ApplicationUser : IdentityUser, IAuditableEntity
{
    [ProtectedPersonalData]
    public string FirstName { get; set; } = default!;
    [ProtectedPersonalData]
    public string LastName { get; set; } = default!;

    public bool IsActive { get; set; }

    [PersonalData]
    public string? DisplayName { get; set; }

    [ProtectedPersonalData]
    public override string Email { get; set; } = default!;

    [PersonalData]
    public string? ProfilePicture { get; set; }

    [PersonalData]
    public string? GravatarEmail { get; set; }

    public DateTime CreatedOn { get; set;  }
    public string? CreatedBy { get; set; }

    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public string? ObjectId { get; set; }

    public ApplicationUser()
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }
}