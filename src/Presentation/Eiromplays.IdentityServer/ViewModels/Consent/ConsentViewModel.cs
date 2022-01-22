// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

namespace Eiromplays.IdentityServer.ViewModels.Consent;

public class ConsentViewModel : ConsentInputModel
{
    public string? ClientName { get; set; }
    public string? ClientUrl { get; set; }
    public string? ClientLogoUrl { get; set; }
    public bool AllowRememberConsent { get; set; }

    public IEnumerable<ScopeViewModel> IdentityScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();
    public IEnumerable<ScopeViewModel> ApiScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();
}