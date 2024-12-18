using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using Netplanety.Shared.Services.Smtp;
using Netplanety.Shared.Tests.Mocks;
using Netplanety.Tests.Common.Mocks;

namespace Netplanety.Shared.Tests;

[TestClass]
public class SmtpServiceTests
{
    private const string User = "Bruno Blanes";
    private const string Address = "bruno.blanes@outlook.com";
    private const string Subject = "This is the message subject";
    private const string Content = "This is the message content";

    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IOptions<SmtpServiceOptions> _options;
    private readonly ILogger<SmtpService> _logger;
    private readonly MockSmtpClient _smtpClient;

    public SmtpServiceTests()
    {
        _smtpClient = new MockSmtpClient();
        _logger = new MockLogger<SmtpService>();
        _cancellationTokenSource = new CancellationTokenSource();
        _options = Options.Create<SmtpServiceOptions>(new SmtpServiceOptions());
    }

    [TestMethod]
    public async Task SendAsync_With_Unauthenticated_Client()
    {
        // Setup the service without an unauthenticated client
        await using var smtpService = new SmtpService(_smtpClient, _logger, _options);
        await smtpService.SendAsync(User, Address, Subject, Content, default);

        // The SmtpClient.SendAsync should not be and
        // called therefore the message should be queued
        Assert.IsFalse(_smtpClient.IsMessageSent);
        Assert.IsTrue(smtpService.Messages.Count is 1);
    }

    [TestMethod]
    public async Task DeQueue_After_Authentication()
    {
        // Setup the service and add a message to the queue
        await using var smtpService = new SmtpService(_smtpClient, _logger, _options);
        smtpService.Messages.Enqueue(new MimeMessage
        {
            Subject = Subject,
            Body = new TextPart { Text = Content }
        });

        // Allow the client to connect
        _smtpClient.CanConnect = true;
        _smtpClient.CanAuthenticate = true;
        DateTime timeout = DateTime.Now.AddSeconds(5);
        while (smtpService.Messages.Count is not 0 && DateTime.Now < timeout)
        {
            await Task.Delay(50);
        }

        // Once connected the queue should be emptyed
        Assert.IsTrue(smtpService.Messages.Count is 0);
    }

    [TestMethod]
    public async Task SendAsync_With_Authenticated_Client()
    {
        // Setup the service with an authenticated client
        _smtpClient.IsAuthenticated = true;
        await using var smtpService = new SmtpService(_smtpClient, _logger, _options);
        await smtpService.SendAsync(User, Address, Subject, Content, default);

        // Since the client is authenticated, the SmtpClient.SendAsync
        // method should be called and therefore no message should be queued.
        Assert.IsTrue(_smtpClient.IsMessageSent);
        Assert.IsTrue(smtpService.Messages.Count is 0);
    }

    [TestMethod]
    public async Task SendAsync_With_Cancellation_Requested()
    {
        // Setup the service with an authenticated client
        // to force the SmtpClient.SendAsync method call
        _smtpClient.IsAuthenticated = true;

        // Cancel the operation
        _cancellationTokenSource.Cancel();
        await using var smtpService = new SmtpService(_smtpClient, _logger, _options);
        async Task sendAsyncFunc() => await smtpService.SendAsync(
            User,
            Address,
            Subject,
            Content,
            _cancellationTokenSource.Token);

        // The OperationCanceledException should be thrown
        await Assert.ThrowsExceptionAsync<OperationCanceledException>(sendAsyncFunc);
    }

    [TestMethod]
    public async Task Reconnect_After_Unrequested_Disconnection()
    {
        // Setup the service
        _smtpClient.CanConnect = true;
        _smtpClient.CanAuthenticate = true;
        await using var smtpService = new SmtpService(_smtpClient, _logger, _options);

        // Invoke the disconnect event
        await _smtpClient.DisconnectAsync(false, default);

        // Allow the client to reconnect
        DateTime timeout = DateTime.Now.AddSeconds(5);
        while (_smtpClient.IsAuthenticated is false && DateTime.Now < timeout)
        {
            await Task.Delay(50);
        }

        // Verify that the client is connected
        Assert.IsTrue(_smtpClient.IsConnected && _smtpClient.IsAuthenticated);
    }

    [TestMethod]
    public async Task ConnectAsync_Handle_SocketException()
    {
        // Setup the service
        _smtpClient.CanConnect = true;
        _smtpClient.CanAuthenticate = true;
        _smtpClient.ThrowSocketException = true;
        await using var smtpService = new SmtpService(_smtpClient, _logger, _options);

        // Allow the client to connect after a SocketException
        DateTime timeout = DateTime.Now.AddSeconds(5);
        while (_smtpClient.IsAuthenticated is false && DateTime.Now < timeout)
        {
            await Task.Delay(50);
        }

        // Verify that the client is connected
        Assert.IsTrue(_smtpClient.IsConnected && _smtpClient.IsAuthenticated);
    }

    [TestMethod]
    public async Task ConnectAsync_And_AuthenticateAsync_Handle_Exception()
    {
        // Setup the service
        _smtpClient.CanConnect = true;
        _smtpClient.CanAuthenticate = true;
        _smtpClient.ThrowConnectException = true;
        _smtpClient.ThrowAuthenticateException = true;
        await using var smtpService = new SmtpService(_smtpClient, _logger, _options);

        // Allow the client to connect after an exception
        DateTime timeout = DateTime.Now.AddSeconds(5);
        while (_smtpClient.IsAuthenticated is false && DateTime.Now < timeout)
        {
            await Task.Delay(50);
        }

        // Verify that the client is connected
        Assert.IsTrue(_smtpClient.IsConnected && _smtpClient.IsAuthenticated);
    }
}
