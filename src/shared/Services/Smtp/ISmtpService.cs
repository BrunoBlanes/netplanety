namespace Netplanety.Shared.Services.Smtp;

/// <summary>
/// Provides methods for sending SMTP messages.
/// </summary>
public interface ISmtpService : IAsyncDisposable
{
	/// <summary>
	/// Sends a new SMTP email message.
	/// </summary>
	/// <param name="user">The <paramref name="user"/> name.</param>
	/// <param name="address">The <paramref name="user"/> email <paramref name="address"/>.</param>
	/// <param name="subject">The message <paramref name="subject"/>.</param>
	/// <param name="content">The message <paramref name="content"/>.</param>
	/// <param name="cancellationToken">
	/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
	/// </param>
	public Task SendAsync(string user, string address, string subject, string content, CancellationToken cancellationToken);
}