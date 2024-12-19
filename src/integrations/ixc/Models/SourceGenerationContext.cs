using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using Netplanety.Integrations.IXC.Extensions.Http;
using Netplanety.Integrations.IXC.Models;

[JsonSerializable(typeof(QueryParams))]
[JsonSerializable(typeof(QueryResult<IXCOnt>))]
[JsonSerializable(typeof(QueryResult<IXCClient>))]
[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
    NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
internal partial class SourceGenerationContext : JsonSerializerContext
{
    public static JsonTypeInfo<QueryResult<T>>? GetJsonTypeInfo<T>(Type type) where T : struct
    {
        MethodInfo? methodInfo = typeof(JsonSerializerContext).GetMethod("GetTypeInfo", [typeof(Type)]);

        return methodInfo is not null ? (JsonTypeInfo<QueryResult<T>>?)methodInfo.Invoke(Default, [type]) : null;
    }
}
