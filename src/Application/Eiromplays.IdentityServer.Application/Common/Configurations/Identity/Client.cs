// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

// Original file: https://github.com/skoruba/Duende.IdentityServer.Admin/
// Modified by Eirik Sjøløkken

namespace Eiromplays.IdentityServer.Application.Common.Configurations.Identity;

public class Client : global::Duende.IdentityServer.Models.Client
{
    public List<Claim> ClientClaims { get; set; } = new();
}