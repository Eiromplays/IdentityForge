// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Eiromplays.IdentityServer.Application.Common.Security;
using Eiromplays.IdentityServer.ViewModels.Grants;
using Microsoft.AspNetCore.Mvc;

namespace Eiromplays.IdentityServer.Controllers;

/// <summary>
/// This sample controller allows a user to revoke grants given to clients
/// </summary>
[SecurityHeaders]
[Microsoft.AspNetCore.Authorization.Authorize]
public class GrantsController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clients;
    private readonly IResourceStore _resources;
    private readonly IEventService _events;

    public GrantsController(IIdentityServerInteractionService interaction,
        IClientStore clients,
        IResourceStore resources,
        IEventService events)
    {
        _interaction = interaction;
        _clients = clients;
        _resources = resources;
        _events = events;
    }
/*
    /// <summary>
    /// Show list of grants
    /// </summary>
    [HttpGet("grants/{clientId}")]
    public async Task<IActionResult> GetGrant(string clientId)
    {
        var grants = await BuildViewModelAsync();

        return Ok(grants.Grants.FirstOrDefault(x => x.ClientId == clientId));
    }
    
    /// <summary>
    /// Get grant for a given clientId
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok(await BuildViewModelAsync());
    }

    /// <summary>
    /// Handle postback to revoke a client
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> Revoke(string clientId)
    {
        await _interaction.RevokeUserConsentAsync(clientId);
        await _events.RaiseAsync(new GrantsRevokedEvent(User.GetSubjectId(), clientId));

        return NoContent();
    }

    private async Task<GrantsViewModel> BuildViewModelAsync()
    {
        var grants = await _interaction.GetAllUserGrantsAsync();

        var list = new List<GrantViewModel>();
        foreach(var grant in grants)
        {
            var client = await _clients.FindClientByIdAsync(grant.ClientId);

            if (client is null) continue;

            var resources = await _resources.FindResourcesByScopeAsync(grant.Scopes);

            var item = new GrantViewModel
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName ?? client.ClientId,
                ClientLogoUrl = client.LogoUri,
                ClientUrl = client.ClientUri,
                Description = grant.Description,
                Created = grant.CreationTime,
                Expires = grant.Expiration,
                IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                ApiGrantNames = resources.ApiScopes.Select(x => x.DisplayName ?? x.Name).ToArray()
            };

            list.Add(item);
        }

        return new GrantsViewModel
        {
            Grants = list
        };
    }*/
}