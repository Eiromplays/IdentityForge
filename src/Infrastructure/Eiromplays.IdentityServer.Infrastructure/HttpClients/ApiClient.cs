using System.Net.Http.Json;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Users.ProfilePicture;

namespace Eiromplays.IdentityServer.Infrastructure.HttpClients;

public class ApiClient : IApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<UpdateProfilePictureResponse?> UpdateProfilePictureAsync(UpdateProfilePictureRequest request)
    {
        using var client = _httpClientFactory.CreateClient("Api");

        using var httpResponseMessage = await client.PostAsJsonAsync("v1/personal/update-profile-picture", request);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return await httpResponseMessage.Content.ReadFromJsonAsync<UpdateProfilePictureResponse>();
        }

        return null;
    }
}