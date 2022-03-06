namespace Eiromplays.IdentityServer.Infrastructure.Extensions;

public static class StringExtensions
{
    public static string GetUniqueFileName(this string fileName)
    {
        return
            $"{Guid.NewGuid().ToString().Replace("-", "")}_{Path.GetFileNameWithoutExtension(fileName)}{Path.GetExtension(fileName)}";
    }
}