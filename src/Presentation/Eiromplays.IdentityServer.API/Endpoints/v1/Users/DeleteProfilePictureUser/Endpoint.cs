using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.DeleteProfilePictureUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    private readonly AccountConfiguration _accountConfiguration;
    
    public Endpoint(IIdentityService identityService, IOptionsMonitor<AccountConfiguration> accountConfigurationOptions)
    {
        _identityService = identityService;
        _accountConfiguration = accountConfigurationOptions.CurrentValue;
    }

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("/users/{id}/profilePicture");
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (!User.IsInRole("Administrator") && !User.HasClaim("sub", req.Id))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        var user = await _identityService.FindUserByIdAsync(req.Id);
        if (user is null)
            ThrowError("User not found");

        if (!_accountConfiguration.ProfilePictureConfiguration.Enabled)
            ThrowError("Profile pictures are currently disabled");

        var profilePicturesPath = Path.Combine(Env.WebRootPath, "Images", "ProfilePictures");
        if (!Directory.Exists(profilePicturesPath))
            Directory.CreateDirectory(profilePicturesPath);

        if (!string.IsNullOrWhiteSpace(user!.ProfilePicture))
        {
            var profilePicturePath = Path.Combine(profilePicturesPath, user.ProfilePicture);
            if (File.Exists(profilePicturePath))
                File.Delete(profilePicturePath);
        }

        user.ProfilePicture = "";
        
        var (result, _) = await _identityService.UpdateUserAsync(user);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();

        await SendNoContentAsync(ct);
    }
}