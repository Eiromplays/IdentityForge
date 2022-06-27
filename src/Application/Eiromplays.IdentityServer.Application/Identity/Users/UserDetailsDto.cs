namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class UserDetailsDto
{
    public string? Id { get; set; }

    public string? UserName { get; set; }
    
    public string? DisplayName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }
    
    public string? GravatarEmail { get; set; }
    
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public bool EmailConfirmed { get; set; }
    
    public bool PhoneNumberConfirmed { get; set; }
    
    public bool TwoFactorEnabled { get; set; }
    
    public bool LockoutEnabled { get; set; } 

    public string? ProfilePicture { get; set; }
    
    public double Credits { get; set; }
    
    public string? DiscordId { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime? LastModifiedOn { get; set; }
}