using Eiromplays.IdentityServer.Application.Identity.Users.ProfilePicture;

namespace Eiromplays.IdentityServer.Application.Common.Interfaces;

public interface IApiClient : ISingletonService
{
    Task<UpdateProfilePictureResponse?> UpdateProfilePictureAsync(UpdateProfilePictureRequest request);
}