// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

// Original file: https://github.com/skoruba/Duende.IdentityServer.Admin/
// Modified by Eirik Sjøløkken

using Duende.IdentityServer.Models;

namespace Eiromplays.IdentityServer.Application.Common.Configurations.Identity;

public class IdentityServerData
{
    public List<Client> Clients { get; set; } = new();
    public List<IdentityResource> IdentityResources { get; set; } = new();
    public List<ApiResource> ApiResources { get; set; } = new();
    public List<ApiScope> ApiScopes { get; set; } = new();
}