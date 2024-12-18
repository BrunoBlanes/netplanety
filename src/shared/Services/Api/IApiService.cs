using Netplanety.Shared.Models;

namespace Netplanety.Shared.Services.Api;

public interface IApiService
{
    /// <summary>
    /// Gets the <see cref="Client"/> with the respective <paramref name="cpf"/>.
    /// </summary>
    /// <param name="cpf">The <see cref="Client"/> <paramref name="cpf"/>.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The <see cref="Client"/> or <c>null</c> if none is found.</returns>
    /// <exception cref="OperationCanceledException"></exception>
    public Task<Client?> GetClientByCpfAsync(string cpf, CancellationToken cancellationToken);
}
