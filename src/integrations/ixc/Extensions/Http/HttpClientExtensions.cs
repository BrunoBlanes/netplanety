using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Netplanety.Shared.Exceptions;

namespace Netplanety.Integrations.IXC.Extensions.Http;

/// <summary>
/// A collection of <see cref="HttpClient"/> extensions.
/// </summary>
internal static class HttpClientExtensions
{
	private static readonly JsonSerializerOptions jsonOptions = new()
	{
		PropertyNameCaseInsensitive = true,
		PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
		NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
	};

	/// <summary>
	/// Get an instance of <typeparamref name="T"/> from IXC API by <paramref name="id"/> or <c>null</c> if none is found.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="id">The <paramref name="id"/> by which the query will be filtered.</param>
	/// <param name="endpoint">The API <paramref name="endpoint"/> the request is sent to.</param>
	/// <param name="cancellationToken">
	/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
	/// </param>
	/// <exception cref="DuplicateIdException"></exception>
	/// <exception cref="DeserializationException"></exception>
	/// <exception cref="OperationCanceledException"></exception>
	/// <returns>The <typeparamref name="T"/> deserialized object.</returns>
	internal static async Task<T?> GetByIdAsync<T>(
		this HttpClient httpClient,
		int id, string endpoint,
		CancellationToken cancellationToken) where T : struct
	{
		// Prepare the endpoint and set headers
		var queryParams = new QueryParams($"{endpoint}.id", $"{id}");
		httpClient.SetIXCSoftHeader(IXCSoftHeaderValues.List);

		// Query the API
		HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
			endpoint,
			queryParams,
			jsonOptions,
			cancellationToken);

		// Process the response body
		var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
		QueryResult<T> queryResult;

		try
		{
			queryResult = JsonSerializer.Deserialize<QueryResult<T>>(content, jsonOptions);
		}

		catch (JsonException)
		{
			// Includes the content for better analysis
			throw new DeserializationException(content);
		}

		if (queryResult.Total > 0)
		{
			if (queryResult.Results.Any())
			{
				try
				{
					// Return the result
					return queryResult.Results.Single();
				}

				// Throw an exception if duplicate ids
				catch (InvalidOperationException)
				{
					throw new DuplicateIdException(id);
				}
			}

			// If total > 0 then there should be results
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