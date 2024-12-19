using System.Text.Json.Serialization;

namespace Netplanety.Shared.Models;

[JsonSerializable(typeof(Client))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate)]
public sealed partial class SourceGenerationContext : JsonSerializerContext
{

}
