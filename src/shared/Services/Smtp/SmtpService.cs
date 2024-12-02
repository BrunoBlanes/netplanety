using System.Net.Sockets;

using MailKit;
using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using Netplanety.Shared.Logging;

namespace Netplanety.Shared.Services.Smtp;

/// <summary>
/// A service that implements <see cref="ISmtpService"/> and sends SMTP messages.
/// </summary>
internal sealed class SmtpService : ISmtpService
{
	private readonly CancellationTokenSource cancellationTokenSource;
	private readonly Queue<MimeMessage> messages;
	private readonly ILogger<SmtpService> logger;
	private readonly MailboxAddress fromAddress;
	private readonly SmtpServiceOptions options;
	private readonly ISmtpClient smtpClient;

	/// <summary>
	/// Initializes a new instance of the <see cref="SmtpService"/> class.
	/// </summary>
	/// <param name="smtpClient">An instance of <see cref="ISmtpClient"/> to send SMTP messages.</param>
	/// <remarks>Creates a new <see cref="SmtpService"/> with the specified <paramref name="options"/>.</remarks>
	/// <param name="logger">The <paramref name="logger"/> which will log messages related to this instance.</param>
	/// <param name="options">The <paramref name="options"/> to configure the <see cref="SmtpService"/>.</param>
	public SmtpService(ISmtpClient smtpClient, ILogger<SmtpService> logger, IOptions<SmtpServiceOptions> options)
	{
		this.logger = logger;
		this.options = options.Value;
		this.smtpClient = smtpClient;

		// Configure the SMTP client
		this.smtpClient.RequireTLS = this.options.RequireTLS;
		this.smtpClient.LocalDomain = this.options.LocalDomain;
		this.smtpClient.CheckCertificateRevocation = this.options.CheckCertificateRevocation;

		messages = new Queue<MimeMessage>();
		cancellationTokenSource = new CancellationTokenSource();
		fromAddress = new MailboxAddress(this.options.FromName, this.options.FromAddress);

		// Initializes the connection and authenticates to the server
		Task.Run(async () => await ConnectAsync(cancellationTokenSource.Token));
		smtpClient.Disconnected += Reconnect;
		smtpClient.Authenticated += DeQueue;
	}

	/// <inheritdoc/>
	public Task SendAsync(string user, string address, string subject, string content, CancellationToken cancellationToken)
	{
		if (cancellationToken == default)
			// If no cancellation token is provided than
			// use local token for cancellation during disposal
			cancellationToken = cancellationTokenSource.Token;

		// Create the MIME message
		var message = new MimeMessage
		{
			Subject = subject,
			Body = new TextPart { Text = content }
		};

		message.From.Add(fromAddress);
		message.To.Add(new MailboxAddress(user, address));

		if (smtpClient.IsAuthenticated)
			// Send the message if the connection is established
			return smtpClient.SendAsync(message, cancellationToken);

		// Enqueue the message to send it once
		// the connection has been established
		messages.Enqueue(message);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Sends queued messages.
	/// </summary>
	/// <param name="_">The <see cref="object"/> sender.</param>
	/// <param name="args">The <see cref="AuthenticatedEventArgs"/>.</param>
	private void DeQueue(object? _, AuthenticatedEventArgs args)
	{
		// Loop for as long as there are messages in the queue and the connection is established
		while (messages.TryPeek(out var message) && smtpClient.IsAuthenticated)
		{
			smtpClient.Send(message, cancellationTokenSource.Token);
			messages.Dequeue();
		}
	}

	/// <summary>
	/// Attempts to reconnect and authenticate to the server.
	/// </summary>
	/// <param name="_">The <see cref="object"/> sender.</param>
	/// <param name="args">The <see cref="DisconnectedEventArgs"/>.</param>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	private async void Reconnect(object? _, DisconnectedEventArgs args)
	{
		await ConnectAsync(cancellationTokenSource.Token);
	}

	/// <summary>
	/// Connects and authenticates to the SMTP server.
	/// </summary>
	/// <param name="cancellationToken">
	/// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	private async Task ConnectAsync(CancellationToken cancellationToken)
	{
		while (smtpClient.IsAuthenticated is false)
		{
			try
			{
				// Attempts to connect and authenticate to the server
				await smtpClient.ConnectAsync(options.Host, options.Port, options.UseSSL, cancellationToken);
				await smtpClient.AuthenticateAsync(options.User, options.Password, cancellationToken);
			}

			catch (SocketException)
			{
				// Socket errors usually indicates a temporary connection issue.
				// Therefore, we wait for a random delay and try again
				await Task.Delay(Random.Shared.Next(1000, 3000), cancellationToken)
					.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
			}

			catch (Exception e) when (e is not ArgumentOutOfRangeException)
			{
				// Log the error and continue
				logger.LogError(e, Messages.Exception, e.GetType().Name, nameof(ConnectAsync));
			}
		}
	}

	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		// Cancel any pending send operation and
		// gracefully close the connection to the server
		await cancellationTokenSource.CancelAsync();
		await smtpClient.DisconnectAsync(true);
		messages.Clear();

		smtpClient.Dispose();
		cancellationTokenSource.Dispose();
		GC.SuppressFinalize(this);
	}
}