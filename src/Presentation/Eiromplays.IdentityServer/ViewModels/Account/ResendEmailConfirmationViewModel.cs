using System.ComponentModel.DataAnnotations;

namespace Eiromplays.IdentityServer.ViewModels.Account;

public class ResendEmailConfirmationViewModel
{
    [Required]
    public string? Identifier { get; set; }
}