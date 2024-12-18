using System.Net.Sockets;

using MailKit;
using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

namespace Netplanety.Shared.Services.Smtp;

/// <summary>
/// A service that implements <see cref="ISmtpService"/> and sends SMTP messages.
/// </summary>
internal sealed class SmtpService : ISmtpService
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ILogger<SmtpService> _logger;
    private readonly MailboxAddress _fromAddress;
    private readonly SmtpServiceOptions _options;
    private readonly ISmtpClient _smtpClient;

    /// <summary>
    /// The message queue.
    /// </summary>
    internal Queue<MimeMessage> Messages { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpService"/> class.
    /// </summary>
    /// <param name="smtpClient">An instance of <see cref="ISmtpClient"/> to send SMTP messages.</param>
    /// <remarks>Creates a new <see cref="SmtpService"/> with the specified <paramref name="options"/>.</remarks>
    /// <param name="logger">The <paramref name="logger"/> which will log messages related to this instance.</param>
    /// <param name="options">The <paramref name="options"/> to configure the <see cref="SmtpService"/>.</param>
    public SmtpService(ISmtpClient smtpClient, ILogger<SmtpService> logger, IOptions<SmtpServiceOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _smtpClient = smtpClient;

        // Configure the SMTP client
        _smtpClient.RequireTLS = _options.RequireTLS;
        _smtpClient.LocalDomain = _options.LocalDomain;
        _smtpClient.CheckCertificateRevocation = _options.CheckCertificateRevocation;

        Messages = new Queue<MimeMessage>();
        _cancellationTokenSource = new CancellationTokenSource();
        _fromAddress = new MailboxAddress(_options.FromName, _options.FromAddress);

        // Initializes the connection and authenticates to the server
        Task.Run(async () => await InitializeAsync(_cancellationTokenSource.Token));
        smtpClient.Disconnected += Reconnect;
        smtpClient.Authenticated += DeQueue;
    }

    /// <inheritdoc/>
    public async Task SendAsync(string user, string address, string subject, string content, CancellationToken cancellationToken)
    {
        if (cancellationToken.CanBeCanceled is false)
        {
            // If no cancellation token is provided than
            // use local token for cancellation during disposal
            cancellationToken = _cancellationTokenSource.Token;
        }

        // Create the MIME message
        var message = new MimeMessage
        {
            Subject = subject,
            Body = new TextPart { Text = content }
        };

        message.From.Add(_fromAddress);
        message.To.Add(new MailboxAddress(user, address));

        if (_smtpClient.IsAuthenticated)
        {
            // Send the message if the connection is established and exit early
            await _smtpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
            return;
        }

        // Enqueue the message to send it once
        // the connection has been established
        Messages.Enqueue(message);
    }

    /// <summary>
    /// Sends queued messages.
    /// </summary>
    /// <param name="_">The <see cref="object"/> sender.</param>
    /// <param name="args">The <see cref="AuthenticatedEventArgs"/>.</param>
    private void DeQueue(object? _, AuthenticatedEventArgs args)
    {
        // Loop for as long as there are messages in the queue and the connection is established
        while (Messages.TryPeek(out MimeMessage? message) && _smtpClient.IsAuthenticated)
        {
            _smtpClient.Send(message, _cancellationTokenSource.Token);
            Messages.Dequeue();
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
        if (args.IsRequested is false)
        {
            await InitializeAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Initializes the connection to the server and authenticates the user.
    /// </summary>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    private async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await ConnectAsync(cancellationToken).ConfigureAwait(false);
        await AuthenticateAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Connects to the SMTP server.
    /// </summary>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private async Task ConnectAsync(CancellationToken cancellationToken)
    {
        while (_smtpClient.IsConnected is false)
        {
            try
            {
                // Attempts to connect the server
                await _smtpClient.ConnectAsync(_options.Host, _options.Port, _options.UseSSL, cancellationToken).ConfigureAwait(false);
                _logger.LogInformation(SmtpLogMessages.Connected, _options.Host, _options.Port);
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
                _logger.LogError(e, Logging.Messages.Exception, e.GetType().Name, nameof(ConnectAsync));
            }
        }
    }

    /// <summary>
    /// Authenticates to the SMTP server.
    /// </summary>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    private async Task AuthenticateAsync(CancellationToken cancellationToken)
    {
        while (_smtpClient.IsAuthenticated is false)
        {
            try
            {
                // Attempts to authenticate with the server
                await _smtpClient.AuthenticateAsync(_options.User, _options.Password, cancellationToken).ConfigureAwait(false);
                _logger.LogInformation(SmtpLogMessages.Authenticated, _options.User);
            }

            catch (Exception e)
            {
                // Log the error and continue
                _logger.LogError(e, Logging.Messages.Exception, e.GetType().Name, nameof(AuthenticateAsync));
            }
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        // Cancel any pending send operation and
        // gracefully close the connection to the server
        await _cancellationTokenSource.CancelAsync();
        await _smtpClient.DisconnectAsync(true);
        _smtpClient.Disconnected -= Reconnect;
        _smtpClient.Authenticated -= DeQueue;
        Messages.Clear();

        _smtpClient.Dispose();
        _cancellationTokenSource.Dispose();
        GC.SuppressFinalize(this);
    }
}
