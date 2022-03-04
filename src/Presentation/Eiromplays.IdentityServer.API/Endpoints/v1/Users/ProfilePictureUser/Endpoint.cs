using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Infrastructure.Extensions;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ProfilePictureUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    
    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/users/{id}/picture");
        Version(1);
        AllowFileUploads();
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Console.WriteLine($"Verify user: {User.HasClaim("sub", req.Id)}");
        
        var user = await _identityService.FindUserByIdAsync(req.Id);
        if (user is null)
            ThrowError("User not found");

        if (req.ProfilePicture is null)
            ThrowError("No profile picture uploaded");
        
        var profilePicturesPath = Path.Combine(Env.WebRootPath, "Images", "ProfilePictures");
        if (!Directory.Exists(profilePicturesPath))
            Directory.CreateDirectory(profilePicturesPath);

        if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
        {
            var oldFilePath = Path.Combine(profilePicturesPath, user.ProfilePicture);
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);
        }

        Console.WriteLine($"File info: {req.ProfilePicture.Name} {req.ProfilePicture.FileName}");
        var fileName = req.ProfilePicture.FileName.GetUniqueFileName();
        var filePath = Path.Combine(profilePicturesPath, fileName);
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await req.ProfilePicture.CopyToAsync(fileStream, ct);
        }
        
        user.ProfilePicture = fileName;
        
        var (result, userId) = await _identityService.UpdateUserAsync(user);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();

        await SendCreatedAtAsync("/users/{id}", userId, new Models.Response{UserDto = user}, ct);
    }
}