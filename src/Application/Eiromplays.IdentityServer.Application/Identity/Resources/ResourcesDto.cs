namespace Eiromplays.IdentityServer.Application.Identity.Resources;

public class ResourcesDto
{
    public ResourcesDto()
    {
    }

    public ResourcesDto(ResourcesDto other)
        : this(other.IdentityResources, other.ApiResources, other.ApiScopes)
    {
        OfflineAccess = other.OfflineAccess;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Resources"/> class.
    /// </summary>
    /// <param name="identityResources">The identity resources.</param>
    /// <param name="apiResources">The API resources.</param>
    /// <param name="apiScopes">The API scopes.</param>
    public ResourcesDto(IEnumerable<IdentityResourceDto>? identityResources, IEnumerable<ApiResourceDto>? apiResources, IEnumerable<ApiScopeDto>? apiScopes)
    {
        var identityResourcesList = identityResources?.ToList();
        if (identityResourcesList?.Any() == true)
        {
            IdentityResources = new HashSet<IdentityResourceDto>(identityResourcesList);
        }

        var apiResourcesList = apiResources?.ToList();
        if (apiResourcesList?.Any() == true)
        {
            ApiResources = new HashSet<ApiResourceDto>(apiResourcesList);
        }

        var apiScopesList = apiScopes?.ToList();
        if (apiScopesList?.Any() == true)
        {
            ApiScopes = new HashSet<ApiScopeDto>(apiScopesList.ToArray());
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [offline access].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [offline access]; otherwise, <c>false</c>.
    /// </value>
    public bool OfflineAccess { get; set; }

    /// <summary>
    /// Gets or sets the identity resources.
    /// </summary>
    public ICollection<IdentityResourceDto> IdentityResources { get; set; } = new HashSet<IdentityResourceDto>();

    /// <summary>
    /// Gets or sets the API resources.
    /// </summary>
    public ICollection<ApiResourceDto> ApiResources { get; set; } = new HashSet<ApiResourceDto>();

    /// <summary>
    /// Gets or sets the API scopes.
    /// </summary>
    public ICollection<ApiScopeDto> ApiScopes { get; set; } = new HashSet<ApiScopeDto>();
}