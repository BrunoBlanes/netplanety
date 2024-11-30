using System.Text.Json.Serialization;

namespace Netplanety.Integrations.IXC.Extensions.Http;

internal readonly struct QueryResult<T> where T : struct
{
	[JsonInclude]
	internal int Page { get; }

	[JsonInclude]
	internal int Total { get; }

	[JsonInclude]
	[JsonPropertyName("registros")]
	internal T[] Results { get; }

	[JsonConstructor]
	internal QueryResult(int page, int total, T[] results)
	{
		Page = page;
		Total = total;
		Results = results ?? [];
	}
}
