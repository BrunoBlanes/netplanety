using System.Net.Http.Json;

using Microsoft.Extensions.Logging;

using Netplanety.Shared.Http;
using Netplanety.Shared.Interfaces;
using Netplanety.Shared.Models;

namespace Netplanety.Shared.Services;

/// <summary>
/// A service for querying the API.
/// </summary>
public sealed class ApiService : IApiService
{
	private readonly ILogger<ApiService> logger;
	private readonly HttpClient httpClient;

	public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
	{
		this.logger = logger;
		this.httpClient = httpClient;
	}

	public async Task<IClient?> GetClientByCpfAsync(string cpf, CancellationToken cancellationToken)
	{
		return await httpClient.GetFromJsonAsync<Client>($"{ApiEndpoints.Client}?cpf={cpf}", cancellationToken);
	}
}