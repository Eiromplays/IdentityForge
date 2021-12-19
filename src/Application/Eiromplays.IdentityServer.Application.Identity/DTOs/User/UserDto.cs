namespace Eiromplays.IdentityServer.Application.Identity.DTOs.User;

public class UserDto
{
    public string? Id { get; set; }

    public string? UserName { get; set; }

    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? GravatarEmail { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public string? ProfilePicture { get; set; }

    public bool LockoutEnabled { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public decimal Credits { get; set; }

    public string? DiscordId { get; set; }
}