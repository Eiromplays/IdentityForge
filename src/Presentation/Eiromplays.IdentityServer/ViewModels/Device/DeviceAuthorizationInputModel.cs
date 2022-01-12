// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using Eiromplays.IdentityServer.ViewModels.Consent;

namespace Eiromplays.IdentityServer.ViewModels.Device;

public class DeviceAuthorizationInputModel : ConsentInputModel
{
    public string? UserCode { get; set; }
}