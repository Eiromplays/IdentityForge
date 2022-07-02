using System.Security.Cryptography;
using System.Text;

namespace Eiromplays.IdentityServer.Infrastructure.Common.Helpers;

public static class Md5HashHelper
{
    public static string GetMd5Hash(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        using var md5 = MD5.Create();
        byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

        var stringBuilder = new StringBuilder();

        foreach (byte t in result)
            stringBuilder.Append(t.ToString("x2"));

        return stringBuilder.ToString();
    }
}