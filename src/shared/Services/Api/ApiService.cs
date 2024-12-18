using System.Net.Http.Json;

using Netplanety.Shared.Http;
using Netplanety.Shared.Models;

namespace Netplanety.Shared.Services.Api;

/// <summary>
/// A service for querying the API.
/// </summary>
public sealed class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Client?> GetClientByCpfAsync(string cpf, CancellationToken cancellationToken)
    {
        Client? client = await _httpClient.GetFromJsonAsync($"{ApiEndpoints.Client}?cpf={cpf}", SourceGenerationContext.Default.Client, cancellationToken);
        return client is null ? null : client;
    }
}
