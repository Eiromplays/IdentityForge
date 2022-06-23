using Eiromplays.IdentityServer.Application.Identity.Auth.Requests;

namespace Eiromplays.IdentityServer.Application.Identity.Auth.Responses.Login;

public class GetLoginResponse
{
    public bool AllowRememberLogin { get; set; } = true;
    public bool EnableLocalLogin { get; set; } = true;

    public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
    public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

    public bool IsExternalLoginOnly => !EnableLocalLogin && ExternalProviders.Count() == 1;
    public string? ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders.SingleOrDefault()?.AuthenticationScheme : null;
    
    public string? Login { get; set; }

    public string ReturnUrl { get; set; } = default!;
}