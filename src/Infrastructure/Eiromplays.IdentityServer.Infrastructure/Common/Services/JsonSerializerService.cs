using Eiromplays.IdentityServer.Application.Common.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eiromplays.IdentityServer.Infrastructure.Common.Services;

public class JsonSerializerService : ISerializerService
{
    public T? Deserialize<T>(string text)
    {
        return JsonSerializer.Deserialize<T>(text);
    }

    public string Serialize<T>(T obj)
    {
        var type = typeof(T);
        return JsonSerializer.Serialize(obj, type, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            });
    }

    public string Serialize<T>(T obj, Type type)
    {
        return JsonSerializer.Serialize(obj, type);
    }
}