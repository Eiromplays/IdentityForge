// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

namespace Eiromplays.IdentityServer.Quickstart.Consent;

public class ConsentInputModel
{
    public string? Button { get; set; }
    public IEnumerable<string> ScopesConsented { get; set; } = Enumerable.Empty<string>();
    public bool RememberConsent { get; set; }
    public string? ReturnUrl { get; set; }
    public string? Description { get; set; }
}