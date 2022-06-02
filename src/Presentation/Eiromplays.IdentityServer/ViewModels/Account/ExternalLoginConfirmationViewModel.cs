// Original file: https://github.com/skoruba/IdentityServer4.Admin
// Modified by Eirik Sjøløkken

using System.ComponentModel.DataAnnotations;

namespace Eiromplays.IdentityServer.ViewModels.Account;

public class ExternalLoginConfirmationViewModel
{
    [Required] public string UserName { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    [Required] public string Email { get; set; } = default!;
}