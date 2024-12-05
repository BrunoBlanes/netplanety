using System.Reflection;

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

	private readonly CancellationTokenSource cancellationTokenSource;
	private readonly IOptions<SmtpServiceOptions> options;
	private readonly ILogger<SmtpService> logger;
	private readonly MockSmtpClient smtpClient;

	public SmtpServiceTests()
	{
		smtpClient = new MockSmtpClient();
		logger = new MockLogger<SmtpService>();
		cancellationTokenSource = new CancellationTokenSource();
		options = Options.Create<SmtpServiceOptions>(new SmtpServiceOptions());
	}

	[TestMethod]
	public async Task SendAsync_With_Unauthenticated_Client()
	{
		// Setup the service without an unauthenticated client
		await using var smtpService = new SmtpService(smtpClient, logger, options);
		await smtpService.SendAsync(User, Address, Subject, Content, default);

		// The SmtpClient.SendAsync should not be and
		// called therefore the message should be queued
		Assert.IsFalse(smtpClient.IsMessageSent);
		Assert.IsTrue(smtpService.Messages.Count is 1);
	}

	[TestMethod]
	public async Task DeQueue_After_Authentication()
	{
		// Setup the service and add a message to the queue
		await using var smtpService = new SmtpService(smtpClient, logger, options);
		smtpService.Messages.Enqueue(new MimeMessage
		{
			Subject = Subject,
			Body = new TextPart { Text = Content }
		});

		// Allow the client to connect
		smtpClient.CanConnect = true;
		smtpClient.CanAuthenticate = true;
		var timeout = DateTime.Now.AddSeconds(5);
		while (smtpService.Messages.Any() && DateTime.Now < timeout)
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
		smtpClient.IsAuthenticated = true;
		await using var smtpService = new SmtpService(smtpClient, logger, options);
		await smtpService.SendAsync(User, Address, Subject, Content, default);

		// Since the client is authenticated, the SmtpClient.SendAsync
		// method should be called and therefore no message should be queued.
		Assert.IsTrue(smtpClient.IsMessageSent);
		Assert.IsFalse(smtpService.Messages.Any());
	}

	[TestMethod]
	public async Task SendAsync_With_Cancellation_Requested()
	{
		// Setup the service with an authenticated client
		// to force the SmtpClient.SendAsync method call
		smtpClient.IsAuthenticated = true;

		// Cancel the operation
		cancellationTokenSource.Cancel();
		await using var smtpService = new SmtpService(smtpClient, logger, options);
		Func<Task> sendAsyncFunc = async () => await smtpService.SendAsync(
			User,
			Address,
			Subject,
			Content,
			cancellationTokenSource.Token);

		// The OperationCanceledException should be thrown
		await Assert.ThrowsExceptionAsync<OperationCanceledException>(sendAsyncFunc);
	}

	[TestMethod]
	public async Task Reconnect_After_Unrequested_Disconnection()
	{
		// Setup the service
		smtpClient.CanConnect = true;
		smtpClient.CanAuthenticate = true;
		await using var smtpService = new SmtpService(smtpClient, logger, options);

		// Invoke the disconnect event
		await smtpClient.DisconnectAsync(false, default);

		// Allow the client to reconnect
		var timeout = DateTime.Now.AddSeconds(5);
		while (smtpClient.IsAuthenticated is false && DateTime.Now < timeout)
		{
			await Task.Delay(50);
		}

		// Verify that the client is connected
		Assert.IsTrue(smtpClient.IsConnected && smtpClient.IsAuthenticated);
	}

	[TestMethod]
	public async Task ConnectAsync_Handle_SocketException()
	{
		// Setup the service
		smtpClient.CanConnect = true;
		smtpClient.CanAuthenticate = true;
		smtpClient.ThrowSocketException = true;
		await using var smtpService = new SmtpService(smtpClient, logger, options);

		// Allow the client to connect after a SocketException
		var timeout = DateTime.Now.AddSeconds(5);
		while (smtpClient.IsAuthenticated is false && DateTime.Now < timeout)
		{
			await Task.Delay(50);
		}

		// Verify that the client is connected
		Assert.IsTrue(smtpClient.IsConnected && smtpClient.IsAuthenticated);
	}

	[TestMethod]
	public async Task ConnectAsync_And_AuthenticateAsync_Handle_Exception()
	{
		// Setup the service
		smtpClient.CanConnect = true;
		smtpClient.CanAuthenticate = true;
		smtpClient.ThrowConnectException = true;
		smtpClient.ThrowAuthenticateException = true;
		await using var smtpService = new SmtpService(smtpClient, logger, options);

		// Allow the client to connect after an exception
		var timeout = DateTime.Now.AddSeconds(5);
		while (smtpClient.IsAuthenticated is false && DateTime.Now < timeout)
		{
			await Task.Delay(50);
		}

		// Verify that the client is connected
		Assert.IsTrue(smtpClient.IsConnected && smtpClient.IsAuthenticated);
	}
}
