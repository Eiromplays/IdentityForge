using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Contracts.v1.Requests.Account;

public class RegisterRequest
{
    public CreateUserRequest Data { get; set; } = default!;
}