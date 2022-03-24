// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

namespace Eiromplays.IdentityServer.API.Grants;

public class GrantsViewModel
{
    public IEnumerable<GrantViewModel> Grants { get; set; } = new List<GrantViewModel>();
}

public class GrantViewModel
{
    public string? ClientId { get; set; }
    public string? ClientName { get; set; }
    public string? ClientUrl { get; set; }
    public string? ClientLogoUrl { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Expires { get; set; }
    public IEnumerable<string> IdentityGrantNames { get; set; } = new List<string>();
    public IEnumerable<string> ApiGrantNames { get; set; } = new List<string>();
}