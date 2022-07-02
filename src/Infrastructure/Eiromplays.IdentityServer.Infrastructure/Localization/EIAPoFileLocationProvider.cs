using Eiromplays.IdentityServer.Infrastructure.Common.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OrchardCore.Localization;

namespace Eiromplays.IdentityServer.Infrastructure.Localization;

/// <summary>
/// Provides PO files for FSH Localization.
/// </summary>
public class EiaPoFileLocationProvider : ILocalizationFileLocationProvider
{
    private readonly IFileProvider _fileProvider;
    private readonly string _resourcesContainer;

    public EiaPoFileLocationProvider(IHostEnvironment hostingEnvironment, IOptions<LocalizationOptions> localizationOptions)
    {
        _fileProvider = hostingEnvironment.ContentRootFileProvider;
        _resourcesContainer = localizationOptions.Value.ResourcesPath;
    }

    public IEnumerable<IFileInfo> GetLocations(string cultureName)
    {
        // Loads all *.po files from the culture folder under the Resource Path.
        // for example, src\Host\Localization\en-US\FSH.Exceptions.po
        return _fileProvider.GetDirectoryContents(PathExtensions.Combine(_resourcesContainer, cultureName));
    }
}
