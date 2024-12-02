using System.Threading;

using MailKit;
using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

namespace Netplanety.Shared.Services;

internal sealed class SmtpService : ISmtpService
{
	private readonly CancellationTokenSource cancellationTokenSource;
	private readonly ILogger<SmtpService> logger;
	private readonly MailboxAddress fromAddress;
	private readonly SmtpServiceOptions options;
	private readonly ISmtpClient smtpClient;

	public SmtpService(ISmtpClient smtpClient, ILogger<SmtpService> logger, IOptions<SmtpServiceOptions> options)
	{
		this.logger = logger;
		this.options = options.Value;
		this.smtpClient = smtpClient;
		this.smtpClient.LocalDomain = this.options.LocalDomain;
		cancellationTokenSource = new CancellationTokenSource();
		fromAddress = new MailboxAddress(this.options.FromName, this.options.FromAddress);

		smtpClient.Disconnected += (_, args) =>
		{
			Reconnect(args, cancellationTokenSource.Token);
		};
	}

	public async Task SendAsync(string name, string to, string subject, string content, CancellationToken cancellationToken)
	{
		if (cancellationToken == default)
		{
			cancellationToken = cancellationTokenSource.Token;
		}

		using var message = new MimeMessage
		{
			Subject = subject,
			Body = new TextPart { Text = content }
		};

		message.From.Add(fromAddress);
		message.To.Add(new MailboxAddress(name, to));

		if (smtpClient.IsConnected == false)
		{
			await ConnectAsync(cancellationToken);
		}

		if (smtpClient.IsAuthenticated == false)
		{
			await AuthenticateAsync(cancellationToken);
		}

		await smtpClient.SendAsync(message, cancellationToken);
	}

	private void Reconnect(DisconnectedEventArgs args, CancellationToken cancellationToken)
	{
		smtpClient.Connect(options.Host, options.Port, options.UseSSL, cancellationToken);
		smtpClient.Authenticate(options.User, options.Password, cancellationToken);
	}

	private Task ConnectAsync(CancellationToken cancellationToken)
	{
		return smtpClient.ConnectAsync(options.Host, options.Port, options.UseSSL, cancellationToken);
	}

	private Task AuthenticateAsync(CancellationToken cancellationToken)
	{
		return smtpClient.AuthenticateAsync(options.User, options.Password, cancellationToken);
	}

	public async ValueTask DisposeAsync()
	{
		await cancellationTokenSource.CancelAsync();
		await smtpClient.DisconnectAsync(true);

		smtpClient.Dispose();
		cancellationTokenSource.Dispose();
		GC.SuppressFinalize(this);
	}
}