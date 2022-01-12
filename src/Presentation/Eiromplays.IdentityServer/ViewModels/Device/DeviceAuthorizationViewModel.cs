// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using Eiromplays.IdentityServer.ViewModels.Consent;

namespace Eiromplays.IdentityServer.ViewModels.Device;

public class DeviceAuthorizationViewModel : ConsentViewModel
{
    public string? UserCode { get; set; }
    public bool ConfirmUserCode { get; set; }
}