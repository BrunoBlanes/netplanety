using Netplanety.Shared.Interfaces;

namespace Netplanety.Shared.Services;

public interface IApiService
{
	/// <summary>
	/// Gets the <see cref="IClient"/> with the respective <paramref name="cpf"/>.
	/// </summary>
	/// <param name="cpf">The <see cref="IClient"/> <paramref name="cpf"/>.</param>
	/// <param name="cancellationToken">
	/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
	/// </param>
	/// <returns>The <see cref="IClient"/> or <c>null</c> if none is found.</returns>
	/// <exception cref="OperationCanceledException"></exception>
	public Task<IClient?> GetClientByCpfAsync(string cpf, CancellationToken cancellationToken);
}