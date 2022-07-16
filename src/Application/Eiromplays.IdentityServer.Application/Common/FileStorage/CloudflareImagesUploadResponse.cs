using System.Text.Json.Serialization;

namespace Eiromplays.IdentityServer.Application.Common.FileStorage;

public class CloudflareImagesUploadResult
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("filename")]
    public string Filename { get; set; } = default!;

    [JsonPropertyName("uploaded")]
    public DateTime Uploaded { get; set; }

    [JsonPropertyName("requireSignedURLs")]
    public bool RequireSignedUrls { get; set; }

    [JsonPropertyName("variants")]
    public List<string> Variants { get; set; } = new();
}

public class CloudflareImagesUploadResponse
{
    [JsonPropertyName("result")]
    public CloudflareImagesUploadResult Result { get; set; } = default!;

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = new();

    [JsonPropertyName("messages")]
    public List<string> Messages { get; set; } = new();
}