// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

namespace Eiromplays.IdentityServer.ViewModels.Account;

public class LogoutViewModel : LogoutInputModel
{
    public bool ShowLogoutPrompt { get; set; } = true;
}