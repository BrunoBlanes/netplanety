namespace Netplanety.Shared.Services;

public interface ISmtpService : IAsyncDisposable
{
	public Task SendAsync(string name, string to, string subject, string content, CancellationToken cancellationToken);
}