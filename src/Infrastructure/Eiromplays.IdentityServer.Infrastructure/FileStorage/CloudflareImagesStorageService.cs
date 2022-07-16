using System.Text.Json;
using System.Text.RegularExpressions;
using Eiromplays.IdentityServer.Application.Common.Caching;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using Eiromplays.IdentityServer.Application.Common.FileStorage;
using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure.Common.Extensions;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.FileStorage;

public class CloudflareImagesStorageService : IFileStorageService
{
    private readonly AccountConfiguration _accountConfiguration;
    private readonly HttpClient _httpClient;

    public CloudflareImagesStorageService(IOptions<CloudflareConfiguration> cloudflareConfiguration, IOptions<AccountConfiguration> accountConfiguration)
    {
        _accountConfiguration = accountConfiguration.Value;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(string.Concat(
                cloudflareConfiguration.Value.ApiBaseUrl,
                cloudflareConfiguration.Value.AccountId,
                cloudflareConfiguration.Value.ImagesBaseUrl)),
            DefaultRequestHeaders = { { "Authorization", $"Bearer {cloudflareConfiguration.Value.ApiToken}" } }
        };
    }

    public async Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
        where T : class
    {
        if (request?.Data is null)
            return string.Empty;

        if ((!_accountConfiguration.ProfilePictureConfiguration.Enabled ||
             _accountConfiguration.ProfilePictureConfiguration.ProfilePictureUploadType is ProfilePictureUploadType
                 .Disabled) && supportedFileType is FileType.ProfilePicture)
        {
            throw new NotSupportedException("Profile picture storage is disabled.");
        }

        if (!supportedFileType.GetDescriptionList().Contains(request.Extension.ToLower()) &&
            !_accountConfiguration.ProfilePictureConfiguration.AllowedFileExtensions.Contains(
                request.Extension.ToLower()) && supportedFileType is FileType.ProfilePicture)
        {
            throw new InvalidOperationException("File Format Not Supported.");
        }

        if (request.Name is null)
            throw new InvalidOperationException("Name is required.");

        string base64Data = Regex.Match(request.Data, "data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

        var response = await _httpClient.PostAsync(
            _httpClient.BaseAddress,
            new MultipartFormDataContent
            {
                { new ByteArrayContent(Convert.FromBase64String(base64Data)), "\"file\"", $"\"{request.Name}{request.Extension}\""}
            },
            cancellationToken);

        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        Console.WriteLine($"Response {responseContent}");
        var responseObject = JsonSerializer.Deserialize<CloudflareImagesUploadResponse>(responseContent);

        Console.WriteLine($"Response Object {JsonSerializer.Serialize(responseObject)}");

        return responseObject?.Result.Variants[0] ?? string.Empty;
    }

    public void Remove(string? path, CancellationToken cancellationToken = default)
    {
        RemoveAsync(path, cancellationToken).GetAwaiter().GetResult();
    }

    public async Task RemoveAsync(string? path, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(path))
            return;

        string[] pathSplit = path.Split('/');

        string imageId = pathSplit[^2];

        await _httpClient.DeleteAsync(new Uri(string.Concat(_httpClient.BaseAddress, $"/{imageId}")), cancellationToken);
    }
}