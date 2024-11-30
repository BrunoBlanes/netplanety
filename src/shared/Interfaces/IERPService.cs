namespace Netplanety.Shared.Interfaces;

public interface IERPService
{
	/// <summary>
	/// Get the <see cref="IFiberTerminal"/> object with the specified <paramref name="id"/>.
	/// </summary>
	/// <param name="id">The <paramref name="id"/> of the <see cref="IFiberTerminal"/> to return.</param>
	/// <param name="cancellationToken">
	/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
	/// </param>
	/// <returns>The <see cref="IFiberTerminal"/> object or <c>null</c> if not found.</returns>
	/// <exception cref="Netplanety.Shared.Exceptions.DuplicateIdException"></exception>
	/// <exception cref="Netplanety.Shared.Exceptions.DeserializationException"></exception>
	/// <exception cref="OperationCanceledException"></exception>
	public Task<IFiberTerminal?> GetFiberTerminalAsync(int id, CancellationToken cancellationToken);
}
