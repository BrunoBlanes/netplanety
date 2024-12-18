using Netplanety.Shared.Exceptions;
using Netplanety.Shared.Models;

namespace Netplanety.Shared.Interfaces;

public interface IERPService
{
	/// <summary>
	/// Get the <see cref="IOnt"/> instance with the specified <paramref name="id"/>.
	/// </summary>
	/// <param name="id">The <paramref name="id"/> of the <see cref="IOnt"/> to return.</param>
	/// <param name="cancellationToken">
	/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
	/// </param>
	/// <returns>The <see cref="IOnt"/> object or <c>null</c> if not found.</returns>
	/// <exception cref="Netplanety.Shared.Exceptions.DuplicateIdException"></exception>
    /// <exception cref="DeserializationException"></exception>
	/// <exception cref="OperationCanceledException"></exception>
	public Task<IOnt?> GetOntAsync(int id, CancellationToken cancellationToken);

	/// <summary>
    /// Get the <see cref="Client"/> instance with the specified <paramref name="cpf"/>.
	/// </summary>
    /// <param name="cpf">The <paramref name="cpf"/> of the <see cref="Client"/>.</param>
	/// <param name="cancellationToken">
	/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
	/// </param>
    /// <returns>The <see cref="Client"/> object or <c>null</c> if not found.</returns>
    /// <exception cref="DuplicateCpfException"></exception>
    /// <exception cref="DeserializationException"></exception>
	/// <exception cref="OperationCanceledException"></exception>
    public Task<Client?> GetClientByCpfAsync(string cpf, CancellationToken cancellationToken);
}
