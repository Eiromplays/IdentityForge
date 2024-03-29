﻿namespace Eiromplays.IdentityServer.Application.Identity.Clients;

public class ClientSecretDto
{
    public int Id { get; set; }

    public string Description { get; set; } = default!;

    public string Value { get; set; } = default!;

    public DateTime? Expiration { get; set; }

    public string Type { get; set; } = "SharedSecret";

    public DateTime Created { get; set; }
}