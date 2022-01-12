using System.ComponentModel.DataAnnotations;

namespace Eiromplays.IdentityServer.Quickstart.Account;

public class ResendEmailConfirmationViewModel
{
    [Required]
    public string? Identifier { get; set; }
}