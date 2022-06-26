using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Mailing;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Domain.Identity;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Eiromplays.IdentityServer.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Authorization;
using ClaimsPrincipalExtensions = Microsoft.Identity.Web.ClaimsPrincipalExtensions;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    /// <summary>
    /// This is used when authenticating with AzureAd.
    /// The local user is retrieved using the objectidentifier claim present in the ClaimsPrincipal.
    /// If no such claim is found, an InternalServerException is thrown.
    /// If no user is found with that ObjectId, a new one is created and populated with the values from the ClaimsPrincipal.
    /// If a role claim is present in the principal, and the user is not yet in that role, then the user is added to that role.
    /// </summary>
    public async Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        var objectId = ClaimsPrincipalExtensions.GetObjectId(principal);
        if (string.IsNullOrWhiteSpace(objectId))
        {
            throw new InternalServerException(_t["Invalid objectId"]);
        }

        var user = await _userManager.Users.Where(u => u.ObjectId == objectId).FirstOrDefaultAsync()
            ?? await CreateOrUpdateFromPrincipalAsync(principal);

        if (principal.FindFirstValue(ClaimTypes.Role) is { } role &&
            await _roleManager.RoleExistsAsync(role) &&
            !await _userManager.IsInRoleAsync(user, role))
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        return user.Id;
    }

    private async Task<ApplicationUser> CreateOrUpdateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        var email = principal.FindFirstValue(ClaimTypes.Upn);
        var username = principal.GetDisplayName();
        
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
        {
            throw new InternalServerException(string.Format(_t["Username or Email not valid."]));
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
        {
            throw new InternalServerException(string.Format(_t["Username {0} is already taken."], username));
        }

        if (user is null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
            {
                throw new InternalServerException(string.Format(_t["Email {0} is already taken."], email));
            }
        }

        IdentityResult? result;
        if (user is not null)
        {
            user.ObjectId = ClaimsPrincipalExtensions.GetObjectId(principal);
            result = await _userManager.UpdateAsync(user);

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        }
        else
        {
            user = new ApplicationUser
            {
                ObjectId = ClaimsPrincipalExtensions.GetObjectId(principal),
                FirstName = principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = principal.FindFirstValue(ClaimTypes.Surname),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            result = await _userManager.CreateAsync(user);

            await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
        }

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Validation Errors Occurred."], result.GetErrors(_t));
        }

        return user;
    }

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

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Validation Errors Occurred."], result.GetErrors(_t));
        }

        if (await _roleManager.RoleExistsAsync(EIARoles.Basic))
            await _userManager.AddToRoleAsync(user, EIARoles.Basic);

        var messages = new List<string> { string.Format(_t["User {0} Registered."], user.UserName) };

        if (_signInManager.Options.SignIn.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
        {
            // send verification email
            var emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
            
            var emailModel = new RegisterUserEmailModel
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = emailVerificationUri
            };
            
            var mailRequest = new MailRequest(
                new List<string> { user.Email },
                _t["Confirm Registration"],
                _templateService.GenerateEmailTemplate("email-confirmation", emailModel));
            
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
            
            messages.Add(_t[$"Please check {user.Email} to verify your account!"]);
        }

        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));

        return new CreateUserResponse(user.Id, string.Join(Environment.NewLine, messages));
    }

    public async Task<CreateUserResponse> CreateExternalAsync(CreateExternalUserRequest request, string origin)
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

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Validation Errors Occurred."], result.GetErrors(_t));
        }

        if (await _roleManager.RoleExistsAsync(EIARoles.Basic))
            await _userManager.AddToRoleAsync(user, EIARoles.Basic);

        var messages = new List<string> { string.Format(_t["User {0} Registered."], user.UserName) };

        if (_signInManager.Options.SignIn.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
        {
            // send verification email
            var emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
            
            var emailModel = new RegisterUserEmailModel
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = emailVerificationUri
            };
            
            var mailRequest = new MailRequest(
                new List<string> { user.Email },
                _t["Confirm Registration"],
                _templateService.GenerateEmailTemplate("email-confirmation", emailModel));
            
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
            
            messages.Add(_t[$"Please check {user.Email} to verify your account!"]);
        }

        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));

        return new CreateUserResponse(user.Id, string.Join(Environment.NewLine, messages));
    }
    
    // TODO: Add support for changing email
    public async Task UpdateAsync(UpdateUserRequest request, string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);
        
        var currentImage = user.ProfilePicture ?? string.Empty;
        if (request.Image is not null || request.DeleteCurrentImage)
        {
            user.ProfilePicture =
                await _fileStorage.UploadAsync<ApplicationUser>(request.Image, FileType.Image, cancellationToken);
            if (request.DeleteCurrentImage && !string.IsNullOrEmpty(currentImage))
            {
                var root = Directory.GetCurrentDirectory();
                _fileStorage.Remove(Path.Combine(root, currentImage));
            }
        }
        
        user.DisplayName = request.DisplayName;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.PhoneNumberConfirmed = request.PhoneNumberConfirmed;
        user.EmailConfirmed = request.EmailConfirmed;
        user.IsActive = request.IsActive;
        user.TwoFactorEnabled = request.TwoFactorEnabled;
        user.LockoutEnabled = request.LockoutEnabled;

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        
        if (request.PhoneNumber != phoneNumber)
        {
            await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
        }

        if (user.GravatarEmail != request.GravatarEmail)
            user.GravatarEmail = request.GravatarEmail;

        var result = await _userManager.UpdateAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        
        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Update profile failed"], result.GetErrors(_t));
        }

        if (request.RevokeUserSessions)
            await RemoveSessionsAsync(userId, cancellationToken);
    }

    public async Task DeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);
        
        var isAdmin = await _userManager.IsInRoleAsync(user, EIARoles.Administrator);
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
}
