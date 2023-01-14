using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.ProfilePicture;
using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Domain.Identity;
using Eiromplays.IdentityServer.Infrastructure.Common.Extensions;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<CreateUserResponse> CreateAsync(CreateUserRequest request, string origin)
    {
        if (_accountConfiguration.RegisterConfiguration is { Enabled: false }) throw new InternalServerException(_t["Registration is disabled."]);

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            DisplayName = !string.IsNullOrWhiteSpace(request.DisplayName) ? request.DisplayName : request.UserName,
            PhoneNumber = request.PhoneNumber,
            IsActive = true
        };

        if (_accountConfiguration.ProfilePictureConfiguration is { Enabled: true, AutoGenerate: true })
            user.ProfilePicture = $"{_accountConfiguration.ProfilePictureConfiguration.DefaultUrl}{user.UserName}.svg";

        var result = string.IsNullOrWhiteSpace(request.Password)
            ? await _userManager.CreateAsync(user)
            : await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Validation Errors Occurred."], result.GetErrors(_t));
        }

        if (await _roleManager.RoleExistsAsync(EIARoles.Basic))
            await _userManager.AddToRoleAsync(user, EIARoles.Basic);

        var messages = new List<string> { string.Format(_t["User {0} Registered."], user.UserName) };

        if (_signInManager.Options.SignIn.RequireConfirmedEmail && !string.IsNullOrWhiteSpace(user.Email))
        {
            string emailVerificationMessage = await SendEmailVerificationAsync(user, origin);

            if (!string.IsNullOrWhiteSpace(emailVerificationMessage))
                messages.Add(emailVerificationMessage);
        }

        if (_signInManager.Options.SignIn.RequireConfirmedPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            string phoneNumberVerificationMessage = await SendPhoneNumberVerificationAsync(user);
            if (!string.IsNullOrWhiteSpace(phoneNumberVerificationMessage))
                messages.Add(phoneNumberVerificationMessage);
        }

        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));

        return new CreateUserResponse(user.Id, string.Join(Environment.NewLine, messages));
    }

    public async Task<UpdateUserResponse> UpdateAsync(UpdateUserRequest request, string userId, string origin, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        string currentImage = user.ProfilePicture ?? string.Empty;
        var updateProfilePictureResponse = await UpdateProfilePictureAsync(new UpdateProfilePictureRequest(currentImage, request.Image), user.Id, cancellationToken);
        user.ProfilePicture = updateProfilePictureResponse.ProfilePictureUrl;

        user.DisplayName = request.DisplayName;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumberConfirmed = request.PhoneNumberConfirmed;
        user.EmailConfirmed = request.EmailConfirmed;
        user.IsActive = request.IsActive;
        user.TwoFactorEnabled = request.TwoFactorEnabled;
        user.LockoutEnabled = request.LockoutEnabled;

        if (user.GravatarEmail != request.GravatarEmail)
            user.GravatarEmail = request.GravatarEmail;

        var messages = new List<string> { string.Format(_t["User {0} Updated."], user.UserName) };

        if (user.PhoneNumber?.Equals(request.PhoneNumber, StringComparison.OrdinalIgnoreCase) == false)
        {
            if (user.PhoneNumberConfirmed)
            {
                user.PhoneNumber = user.PhoneNumber;
            }
            else
            {
                string phoneNumberVerificationMessage = await SendPhoneNumberVerificationAsync(user);
                if (!string.IsNullOrWhiteSpace(phoneNumberVerificationMessage))
                    messages.Add(phoneNumberVerificationMessage);
            }
        }

        if (user.Email?.Equals(request.Email, StringComparison.OrdinalIgnoreCase) == false)
        {
            if (user.EmailConfirmed)
            {
                user.Email = request.Email;
            }
            else
            {
                string emailVerificationMessage = await SendEmailVerificationAsync(user, origin);
                if (!string.IsNullOrWhiteSpace(emailVerificationMessage))
                    messages.Add(emailVerificationMessage);
            }
        }

        var result = await _userManager.UpdateAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Update profile failed"], result.GetErrors(_t));
        }

        if (request.RevokeUserSessions)
            await RemoveBffSessionsAsync(userId, cancellationToken);

        return new UpdateUserResponse { Message = string.Join(Environment.NewLine, messages) };
    }

    public async Task<UpdateProfileResponse> UpdateProfileAsync(UpdateProfileRequest request, string userId, string origin, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        var apiClient = _serviceProvider.GetService<IApiClient>();

        string currentImage = user.ProfilePicture ?? string.Empty;
        if (apiClient is not null)
        {
            var updateProfilePictureResponse = await apiClient.UpdateProfilePictureAsync(
                new UpdateProfilePictureRequest(currentImage, request.Image));

            if (updateProfilePictureResponse is not null)
                user.ProfilePicture = updateProfilePictureResponse.ProfilePictureUrl;
        }

        user.DisplayName = request.DisplayName;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        if (user.GravatarEmail != request.GravatarEmail)
            user.GravatarEmail = request.GravatarEmail;

        var messages = new List<string> { string.Format(_t["User {0} Updated."], user.UserName) };

        var response = new UpdateProfileResponse { Message = string.Join(Environment.NewLine, messages) };

        if (user.PhoneNumber?.Equals(request.PhoneNumber, StringComparison.OrdinalIgnoreCase) == false && !string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            user.PhoneNumberConfirmed = false;
            user.PhoneNumber = request.PhoneNumber;
            response.LogoutRequired = true;

            if (_signInManager.Options.SignIn.RequireConfirmedPhoneNumber)
            {
                string phoneNumberVerificationMessage = await SendPhoneNumberVerificationAsync(user);
                if (!string.IsNullOrWhiteSpace(phoneNumberVerificationMessage))
                    messages.Add(phoneNumberVerificationMessage);

                response.ReturnUrl =
                    $"{_urlConfiguration.IdentityServerUiBaseUrl}auth/verify-phone-number?userId={user.Id}";
            }
        }

        if (user.Email?.Equals(request.Email, StringComparison.OrdinalIgnoreCase) == false && !string.IsNullOrWhiteSpace(request.Email))
        {
            user.EmailConfirmed = false;
            response.LogoutRequired = true;

            if (_signInManager.Options.SignIn.RequireConfirmedEmail)
            {
                string emailVerificationMessage = await SendEmailVerificationAsync(user, origin);
                if (!string.IsNullOrWhiteSpace(emailVerificationMessage))
                    messages.Add(emailVerificationMessage);
            }
            else
            {
                user.Email = request.Email;
            }
        }

        var result = await _userManager.UpdateAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Update profile failed"], result.GetErrors(_t));
        }

        if (request.RevokeUserSessions)
            await RemoveBffSessionsAsync(userId, cancellationToken);

        return response;
    }

    public async Task DeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        bool isAdmin = await _userManager.IsInRoleAsync(user, EIARoles.Administrator);
        if (isAdmin)
        {
            throw new ConflictException(_t["Administrators Profile's cannot be deleted"]);
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Delete user failed"], result.GetErrors(_t));
        }

        await _signInManager.SignOutAsync();

        await _events.PublishAsync(new ApplicationUserDeletedEvent(user.Id));
    }

    #region Profile Pictures

    /// <summary>
    /// TODO: Should probably update the user's profile picture in the database as well.
    /// As we do not want to rely on the APIs doing this for us.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UpdateProfilePictureResponse> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId, CancellationToken cancellationToken)
    {
        if (request.Image is not null)
        {
            request.Image.Name = $"{userId}_{request.Image.Name}";
        }

        if (!string.IsNullOrWhiteSpace(request.OldProfilePicturePath))
        {
            if (!request.OldProfilePicturePath.IsValidUri())
            {
                string root = Directory.GetCurrentDirectory();
                await _fileStorage.RemoveAsync(Path.Combine(root, request.OldProfilePicturePath), cancellationToken);
            }
            else
            {
                await _fileStorage.RemoveAsync(request.OldProfilePicturePath, cancellationToken);
            }
        }

        var response = new UpdateProfilePictureResponse
        {
            ProfilePictureUrl =
                await _fileStorage.UploadAsync<ApplicationUser>(request.Image, FileType.ProfilePicture, cancellationToken)
        };

        return response;
    }

    #endregion
}
