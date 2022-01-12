// Original file: https://github.com/skoruba/IdentityServer4.Admin
// Modified by Eirik Sjøløkken

using System.ComponentModel.DataAnnotations;

namespace Eiromplays.IdentityServer.Quickstart.Account;

public class ExternalLoginConfirmationViewModel
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? DisplayName { get; set; }

    [Required]
    public string? Email { get; set; }
}