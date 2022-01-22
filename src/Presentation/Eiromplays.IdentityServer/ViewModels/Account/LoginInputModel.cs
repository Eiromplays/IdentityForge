// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using System.ComponentModel.DataAnnotations;

namespace Eiromplays.IdentityServer.ViewModels.Account;

public class LoginInputModel
{
    [Required]
    public string? Login { get; set; }

    [Required]
    public string? Password { get; set; }
    public bool RememberLogin { get; set; }
    public string? ReturnUrl { get; set; }
}