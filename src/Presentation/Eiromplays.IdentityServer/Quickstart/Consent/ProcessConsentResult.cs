// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using Duende.IdentityServer.Models;

namespace Eiromplays.IdentityServer.Quickstart.Consent;

public class ProcessConsentResult
{
    public bool IsRedirect => RedirectUri is not null;
    public string? RedirectUri { get; set; }
    public Client? Client { get; set; }

    public bool ShowView => ViewModel is not null;
    public ConsentViewModel? ViewModel { get; set; }

    public bool HasValidationError => ValidationError is not null;
    public string? ValidationError { get; set; }
}