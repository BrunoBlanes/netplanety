using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using MailKit;
using MailKit.Net.Proxy;
using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

namespace Netplanety.Shared.Tests.Mocks;

internal class MockSmtpClient : ISmtpClient
{
    public bool CanConnect { get; set; }
    public bool CanAuthenticate { get; set; }
    public bool ThrowSocketException { get; set; }
    public bool ThrowConnectException { get; set; }
    public bool ThrowAuthenticateException { get; set; }
    public bool IsConnected { get; set; }
    public bool IsAuthenticated { get; set; }
    public bool IsMessageSent { get; set; }
    public bool RequireTLS { get; set; }
    public string LocalDomain { get; set; }
    public bool CheckCertificateRevocation { get; set; }

    public MockSmtpClient()
    {
        IsMessageSent = false;
        LocalDomain = string.Empty;
        ClientCertificates = [];
    }

    public SmtpCapabilities Capabilities => throw new NotImplementedException();


    public uint MaxSize => throw new NotImplementedException();

    public DeliveryStatusNotificationType DeliveryStatusNotificationType { get; set; }

    public object SyncRoot => throw new NotImplementedException();

    public SslProtocols SslProtocols { get; set; }
    public CipherSuitesPolicy SslCipherSuitesPolicy { get; set; }

    public TlsCipherSuite? SslCipherSuite => throw new NotImplementedException();

    public X509CertificateCollection ClientCertificates { get; set; }
    public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IPEndPoint LocalEndPoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IProxyClient ProxyClient { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public HashSet<string> AuthenticationMechanisms => throw new NotImplementedException();


    public bool IsSecure => throw new NotImplementedException();

    public bool IsEncrypted => throw new NotImplementedException();

    public bool IsSigned => throw new NotImplementedException();

    public SslProtocols SslProtocol => throw new NotImplementedException();

    public CipherAlgorithmType? SslCipherAlgorithm => throw new NotImplementedException();

    public int? SslCipherStrength => throw new NotImplementedException();

    public HashAlgorithmType? SslHashAlgorithm => throw new NotImplementedException();

    public int? SslHashStrength => throw new NotImplementedException();

    public ExchangeAlgorithmType? SslKeyExchangeAlgorithm => throw new NotImplementedException();

    public int? SslKeyExchangeStrength => throw new NotImplementedException();

    public int Timeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event EventHandler<MessageSentEventArgs>? MessageSent;
    public event EventHandler<ConnectedEventArgs>? Connected;
    public event EventHandler<DisconnectedEventArgs>? Disconnected;
    public event EventHandler<AuthenticatedEventArgs>? Authenticated;

    public void Authenticate(ICredentials credentials, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Authenticate(Encoding encoding, ICredentials credentials, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Authenticate(Encoding encoding, string userName, string password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Authenticate(string userName, string password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Authenticate(SaslMechanism mechanism, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AuthenticateAsync(ICredentials credentials, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AuthenticateAsync(Encoding encoding, ICredentials credentials, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AuthenticateAsync(Encoding encoding, string userName, string password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        if (IsConnected is false)
        {
            throw new ServiceNotConnectedException();
        }

        if (IsAuthenticated)
        {
            throw new InvalidOperationException();
        }

        if (ThrowAuthenticateException)
        {
            ThrowAuthenticateException = false;
            throw new Exception();
        }

        IsAuthenticated = CanAuthenticate;
        Authenticated?.Invoke(this, new AuthenticatedEventArgs(string.Empty));
        return Task.CompletedTask;
    }

    public Task AuthenticateAsync(SaslMechanism mechanism, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Connect(string host, int port, bool useSsl, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Connect(string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Connect(Socket socket, string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Connect(Stream stream, string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken = default)
    {
        if (IsConnected)
        {
            throw new InvalidOperationException();
        }

        if (ThrowSocketException)
        {
            ThrowSocketException = false;
            throw new SocketException();
        }

        if (ThrowConnectException)
        {
            ThrowConnectException = false;
            throw new Exception();
        }

        IsConnected = CanConnect;
        Connected?.Invoke(this, new ConnectedEventArgs(host, port, SecureSocketOptions.None));
        return Task.CompletedTask;
    }

    public Task ConnectAsync(string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ConnectAsync(Socket socket, string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ConnectAsync(Stream stream, string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Disconnect(bool quit, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DisconnectAsync(bool quit, CancellationToken cancellationToken = default)
    {
        IsConnected = false;
        IsAuthenticated = false;
        Disconnected?.Invoke(this, new DisconnectedEventArgs(string.Empty, default, SecureSocketOptions.None, quit));
        return Task.CompletedTask;
    }

    public void Dispose()
    {

    }

    public InternetAddressList Expand(string alias, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<InternetAddressList> ExpandAsync(string alias, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void NoOp(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task NoOpAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public string Send(MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress? progress = null)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IsMessageSent = true;
        MessageSent?.Invoke(this, new MessageSentEventArgs(message, string.Empty));
        return string.Empty;
    }

    public string Send(MimeMessage message, MailboxAddress sender, IEnumerable<MailboxAddress> recipients, CancellationToken cancellationToken = default, ITransferProgress? progress = null)
    {
        throw new NotImplementedException();
    }

    public string Send(FormatOptions options, MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress? progress = null)
    {
        throw new NotImplementedException();
    }

    public string Send(FormatOptions options, MimeMessage message, MailboxAddress sender, IEnumerable<MailboxAddress> recipients, CancellationToken cancellationToken = default, ITransferProgress? progress = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> SendAsync(MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress? progress = null)
    {
        return Task.FromResult<string>(Send(message, cancellationToken, progress));
    }

    public Task<string> SendAsync(MimeMessage message, MailboxAddress sender, IEnumerable<MailboxAddress> recipients, CancellationToken cancellationToken = default, ITransferProgress? progress = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> SendAsync(FormatOptions options, MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress? progress = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> SendAsync(FormatOptions options, MimeMessage message, MailboxAddress sender, IEnumerable<MailboxAddress> recipients, CancellationToken cancellationToken = default, ITransferProgress? progress = null)
    {
        throw new NotImplementedException();
    }

    public MailboxAddress Verify(string address, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<MailboxAddress> VerifyAsync(string address, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
