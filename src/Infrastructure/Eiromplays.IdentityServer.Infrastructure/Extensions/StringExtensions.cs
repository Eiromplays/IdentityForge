namespace Eiromplays.IdentityServer.Infrastructure.Extensions;

public static class StringExtensions
{
    public static string GetUniqueFileName(this string fileName)
    {
        return
            $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid().ToString()[..4]}{Path.GetExtension(fileName)}";
    }
}