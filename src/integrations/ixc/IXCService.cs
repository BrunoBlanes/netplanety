using System.Net.Http.Headers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Netplanety.Integrations.IXC.Extensions.Http;
using Netplanety.Integrations.IXC.Models;
using Netplanety.Shared.Exceptions;
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

	public IXCService(ILogger<IXCService> logger, HttpClient httpClient, IOptions<IXCServiceOptions> options)
	{
		this.logger = logger;
		this.httpClient = httpClient;

		// Configure the API client
		this.httpClient.BaseAddress = new Uri(options.Value.HttpClientOptions.BaseAddress);
		this.httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {options.Value.HttpClientOptions.Token}");
		this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	public async Task<IOnt?> GetOntAsync(int id, CancellationToken cancellationToken)
	{
		IXCOnt? fiberClient = await httpClient.GetByIdAsync<IXCOnt>(id, QueryEndpoints.FiberClient, cancellationToken);
		return fiberClient?.ToOnt();
	}

	public async Task<IClient?> GetClientByCpfAsync(string cpf, CancellationToken cancellationToken)
	{
		IXCClient? client;

		try
		{
			client = await httpClient.GetSingleAsync<IXCClient>(QueryEndpoints.Client, QueryFilters.CPF, cpf, cancellationToken);
			return client?.ToClient();
		}

		catch (InvalidOperationException)
		{
			throw new DuplicateCpfException(cpf);
		}
	}

	private void Dispose(bool disposing)
	{
		if (disposedValue is false)
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