using System.ComponentModel.DataAnnotations;

namespace Eiromplays.IdentityServer.ViewModels.Account;

public class RegisterViewModel
{
    public string? UserName { get; set; }

    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}