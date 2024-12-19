using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using Netplanety.Shared.Exceptions;

namespace Netplanety.Integrations.IXC.Extensions.Http;

/// <summary>
/// A collection of <see cref="HttpClient"/> extensions.
/// </summary>
internal static class HttpClientExtensions
{
    /// <summary>
    /// Gets an instance of <typeparamref name="T"/> from IXC API by <paramref name="id"/> or <c>null</c> if none is found.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
    /// <param name="id">The <paramref name="id"/> by which the query will be filtered.</param>
    /// <param name="endpoint">The API <paramref name="endpoint"/> the request is sent to.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <exception cref="DuplicateIdException"></exception>
    /// <exception cref="DeserializationException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    /// <returns>The single <typeparamref name="T"/> deserialized object.</returns>
    internal static async Task<T?> GetByIdAsync<T>(
        this HttpClient httpClient,
        int id,
        string endpoint,
        CancellationToken cancellationToken) where T : struct
    {
        try
        {
            return await httpClient.GetSingleAsync<T>(endpoint, QueryFilters.Id, id.ToString(), cancellationToken);
        }

        // Throw an exception if duplicate ids
        catch (InvalidOperationException)
        {
            throw new DuplicateIdException(id);
        }
    }

    /// <summary>
    /// Gets a single instance of <typeparamref name="T"/> from IXC API.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
    /// <param name="endpoint">The API <paramref name="endpoint"/> the request is sent to.</param>
    /// <param name="queryFilter">The field by which the query will be filtered.</param>
    /// <param name="queryParam">The value used to filter the query.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The single <typeparamref name="T"/> deserialized object.</returns>
    /// <exception cref="DeserializationException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    internal static async Task<T?> GetSingleAsync<T>(
        this HttpClient httpClient,
        string endpoint,
        string queryFilter,
        string queryParam,
        CancellationToken cancellationToken) where T : struct
    {
        T[]? results = await httpClient.GetAsync<T>(endpoint, queryFilter, queryParam, cancellationToken);
        return results?.Single();
    }

    /// <summary>
    /// Get an instance of <typeparamref name="T"/> from IXC API by <paramref name="id"/> or <c>null</c> if none is found.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
    /// <param name="endpoint">The API <paramref name="endpoint"/> the request is sent to.</param>
    /// <param name="queryFilter">The field by which the query will be filtered.</param>
    /// <param name="queryParam">The value used to filter the query.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>An array of <typeparamref name="T"/> or <c>null</c> if no results are found.</returns>
    /// <exception cref="DeserializationException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    internal static async Task<T[]?> GetAsync<T>(
        this HttpClient httpClient,
        string endpoint,
        string queryFilter,
        string queryParam,
        CancellationToken cancellationToken) where T : struct
    {
        // Prepare the endpoint and set headers
        var queryParams = new QueryParams($"{endpoint}.{queryFilter}", $"{queryParam}");
        httpClient.SetIXCSoftHeader(IXCSoftHeaderValues.List);

        // Query the API
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            endpoint,
            queryParams,
            SourceGenerationContext.Default.QueryParams,
            cancellationToken);

        // Process the response body
        string content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        JsonTypeInfo<QueryResult<T>>? jsonTypeInfo = SourceGenerationContext.GetJsonTypeInfo<T>(typeof(QueryResult<T>));
        QueryResult<T> queryResult;

        try
        {
            if (jsonTypeInfo is not null)
            {
                queryResult = JsonSerializer.Deserialize(content, jsonTypeInfo);

                if (queryResult.Total > 0)
                {
                    if (queryResult.Results.Length is not 0)
                    {
                        // Return the result
                        return queryResult.Results;
                    }

                    // If total > 0 then there should be results
                    throw new DeserializationException(content);
                }
            }
        }

        catch (JsonException)
        {
            // Includes the content for better analysis
            throw new DeserializationException(content);
        }

        return null;
    }

    /// <summary>
    /// Set the <c>ixcsoft</c> header value.
    /// </summary>
    /// <param name="headerValue">The <c>ixcsoft</c> header value.</param>
    private static void SetIXCSoftHeader(this HttpClient httpClient, string headerValue)
    {
        httpClient.DefaultRequestHeaders.Remove("ixcsoft");
        httpClient.DefaultRequestHeaders.Add("ixcsoft", headerValue);
    }
}
