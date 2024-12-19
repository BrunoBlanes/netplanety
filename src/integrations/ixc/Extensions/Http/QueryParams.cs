using System.Text.Json.Serialization;

namespace Netplanety.Integrations.IXC.Extensions.Http;

internal readonly struct QueryParams
{
    [JsonInclude]
    [JsonPropertyName("qtype")]
    internal string Type { get; }

    [JsonInclude]
    internal string Query { get; }

    [JsonInclude]
    [JsonPropertyName("oper")]
    internal QueryOperation Operation { get; }

    [JsonInclude]
    internal int Page { get; }

    [JsonInclude]
    [JsonPropertyName("rp")]
    internal int PageSize { get; }

    [JsonInclude]
    internal string SortName { get; }

    [JsonInclude]
    internal QuerySortOrder SortOrder { get; }

    /// <summary>
    /// Create a new <see cref="QueryParams"/>.
    /// </summary>
    /// <param name="queryType">The table column to query for.</param>
    /// <param name="query">The value to query.</param>
    /// <param name="operation">The comparison operator. Defaults to <see cref="QueryOperation.Equals"/>.</param>
    /// <param name="page">The page number. Defaults to <c>1</c>, which is the first page.</param>
    /// <param name="pageSize">The number of results per page. Defaults to <c>10</c>.</param>
    /// <param name="sortName">
    /// The field by which results will be ordered.
    /// Defaults to same value as <paramref name="queryType"/>.
    /// </param>
    /// <param name="sortOrder">The sort order. Defaults to <see cref="QuerySortOrder.Decrescent"/>.</param>
    internal QueryParams(
        string queryType,
        string query,
        int page = 1,
        QueryOperation operation = QueryOperation.Equals,
        int pageSize = 10,
        string? sortName = null,
        QuerySortOrder sortOrder = QuerySortOrder.Decrescent)
    {
        Page = page;
        Query = query;
        Type = queryType;
        SortName = sortName ?? queryType;
        PageSize = pageSize;
        Operation = operation;
        SortOrder = sortOrder;
    }
}

[JsonConverter(typeof(JsonStringEnumConverter<QueryOperation>))]
internal enum QueryOperation
{
    [JsonStringEnumMemberName("<")]
    LessThan,

    [JsonStringEnumMemberName("=")]
    Equals,

    [JsonStringEnumMemberName(">")]
    BiggerThan
}

[JsonConverter(typeof(JsonStringEnumConverter<QuerySortOrder>))]
internal enum QuerySortOrder
{
    [JsonStringEnumMemberName("asc")]
    Crescent,

    [JsonStringEnumMemberName("desc")]
    Decrescent
}