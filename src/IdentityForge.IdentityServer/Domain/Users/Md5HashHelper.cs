using System.Security.Cryptography;
using System.Text;

namespace IdentityForge.IdentityServer.Domain.Users;

public static class Md5HashHelper
{
    public static string GetMd5Hash(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        byte[] result = MD5.HashData(Encoding.UTF8.GetBytes(input));

        var stringBuilder = new StringBuilder();

        foreach (byte t in result)
            stringBuilder.Append(t.ToString("x2"));

        return stringBuilder.ToString();
    }
}