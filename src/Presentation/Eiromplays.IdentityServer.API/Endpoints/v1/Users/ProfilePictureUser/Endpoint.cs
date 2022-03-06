using System.Net;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Extensions;
using FastEndpoints;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ProfilePictureUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    private readonly AccountConfiguration _accountConfiguration;
    private readonly HttpContext? _currentContext;

    public Endpoint(IIdentityService identityService, IOptionsMonitor<AccountConfiguration> accountConfigurationOptions,
        IHttpContextAccessor httpContextAccessor)
    {
        _identityService = identityService;
        _accountConfiguration = accountConfigurationOptions.CurrentValue;
        _currentContext = httpContextAccessor.HttpContext;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/users/{id}/profilePicture");
        Version(1);
        AllowFileUploads();
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (!User.IsInRole("Administrator") && !User.HasClaim("sub", req.Id))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        Console.WriteLine($"Host: {HttpContext.Request.Host.Value}");
        var user = await _identityService.FindUserByIdAsync(req.Id);
        if (user is null)
            ThrowError("User not found");

        if (!_accountConfiguration.ProfilePictureConfiguration.Enabled ||
            _accountConfiguration.ProfilePictureConfiguration.ProfilePictureUploadType ==
            ProfilePictureUploadType.Disabled)
            ThrowError("Profile picture upload is disabled");

        if (!_accountConfiguration.ProfilePictureConfiguration.AllowedFileExtensions.Contains(
                Path.GetExtension(req.ProfilePicture.FileName).StartsWith('.')
                    ? Path.GetExtension(req.ProfilePicture.FileName)
                    : $".{Path.GetExtension(req.ProfilePicture.FileName)}"))
            ThrowError($"File extension not supported, the currently supported file types are: {string.Join(", ", _accountConfiguration.ProfilePictureConfiguration.AllowedFileExtensions)}");;

        var profilePicturesPath = Path.Combine(Env.WebRootPath, "Images", "ProfilePictures");
        if (!Directory.Exists(profilePicturesPath))
            Directory.CreateDirectory(profilePicturesPath);

        if (!string.IsNullOrWhiteSpace(user!.ProfilePicture))
        {
            var oldFilePath = Path.Combine(profilePicturesPath, user.ProfilePicture);
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);
        }

        var fileName = "";
        switch (_accountConfiguration.ProfilePictureConfiguration.ProfilePictureUploadType)
        {
            case ProfilePictureUploadType.File:
               fileName = req.ProfilePicture.FileName.GetUniqueFileName();
                var filePath = Path.Combine(profilePicturesPath, fileName);
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await req.ProfilePicture.CopyToAsync(fileStream, ct);
                }
               break;
            case ProfilePictureUploadType.Base64:
                await using (var memoryStream = new MemoryStream())
                {
                    await req.ProfilePicture.CopyToAsync(memoryStream, ct);
                    var fileBytes = memoryStream.ToArray();
                    fileName = Convert.ToBase64String(fileBytes);
                }
                break;
        }

        if (string.IsNullOrWhiteSpace(fileName))
            ThrowError("Failed to upload profile picture");
        
        user.ProfilePicture = fileName;
        
        var (result, userId) = await _identityService.UpdateUserAsync(user);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();

        await SendCreatedAtAsync("/users/{id}", userId, new Models.Response{UserDto = user}, ct);
    }
}