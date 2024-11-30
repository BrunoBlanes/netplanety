using System.Net.Http.Headers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Netplanety.Integrations.IXC.Extensions.Http;
using Netplanety.Integrations.IXC.Models;
using Netplanety.Shared.Interfaces;

namespace Netplanety.Integrations.IXC;

/// <summary>
/// A service that intent to provide means of exchanging information between this program and the IXC API.
/// </summary>
/// <remarks>For more information go to: <see href="https://wikiapiprovedor.ixcsoft.com.br/"/></remarks>
internal sealed class IXCService : IERPService, IDisposable
{
	private readonly ILogger<IXCService> logger;
	private readonly HttpClient httpClient;
	private bool disposedValue;

	internal IXCService(ILogger<IXCService> logger, HttpClient httpClient, IOptions<IXCServiceOptions> options)
	{
		this.logger = logger;
		this.httpClient = httpClient;

		// Configure the API client
		this.httpClient.BaseAddress = new Uri(options.Value.HttpClientOptions.BaseAddress);
		this.httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {options.Value.HttpClientOptions.Token}");
		this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	public async Task<IFiberTerminal?> GetFiberTerminalAsync(int id, CancellationToken cancellationToken)
	{
		FiberClient? fiberClient = await GetByIdAsync<FiberClient>(id, Endpoints.FiberClient, cancellationToken);
		return fiberClient?.ToFiberTerminal();
	}

	/// <summary>
	/// Get the <typeparamref name="T"/> object with the specified <paramref name="id"/>.
	/// </summary>
	/// <typeparam name="T">The object type.</typeparam>
	/// <param name="id">The <paramref name="id"/> of the <typeparamref name="T"/> to return.</param>
	/// <param name="endpoint">The API <paramref name="endpoint"/> the request is sent to.</param>
	/// <param name="cancellationToken">
	/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
	/// </param>
	/// <returns>The <typeparamref name="T"/> object or <c>null</c> if not found.</returns>
	/// <exception cref="Netplanety.Shared.Exceptions.DuplicateIdException"></exception>
	/// <exception cref="Netplanety.Shared.Exceptions.DeserializationException"></exception>
	/// <exception cref="OperationCanceledException"></exception>
	private async Task<T?> GetByIdAsync<T>(int id, string endpoint, CancellationToken cancellationToken) where T: struct
	{
		return await httpClient.GetByIdAsync<T>(id, endpoint, cancellationToken);
	}

	private void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				httpClient.Dispose();
			}

			disposedValue = true;
		}
	}

	void IDisposable.Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}