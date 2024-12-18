using System.Net.Http.Headers;

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
    private readonly HttpClient _httpClient;
    private bool _disposedValue;

    public IXCService(HttpClient httpClient, IOptions<IXCServiceOptions> options)
	{
        _httpClient = httpClient;

		// Configure the API client
        _httpClient.BaseAddress = new Uri(options.Value.HttpClientOptions.BaseAddress);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {options.Value.HttpClientOptions.Token}");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	public async Task<IOnt?> GetOntAsync(int id, CancellationToken cancellationToken)
	{
		IXCOnt? fiberClient = await httpClient.GetByIdAsync<IXCOnt>(id, QueryEndpoints.FiberClient, cancellationToken);
		return fiberClient?.ToOnt();
	}

	public async Task<IClient?> GetClientByCpfAsync(string cpf, CancellationToken cancellationToken)
	{
        IClient? client;

		try
		{
            client = await _httpClient.GetSingleAsync<IXCClient>(
                QueryEndpoints.Client,
                QueryFilters.CPF,
                cpf,
                cancellationToken);
		}

		catch (InvalidOperationException)
		{
			throw new DuplicateCpfException(cpf);
		}
	}

	private void Dispose(bool disposing)
	{
        if (_disposedValue is false)
		{
			if (disposing)
			{
                _httpClient.Dispose();
			}

            _disposedValue = true;
		}
	}

	void IDisposable.Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
