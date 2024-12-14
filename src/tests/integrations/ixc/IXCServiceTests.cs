using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Netplanety.Integrations.IXC.Extensions.Http;
using Netplanety.Integrations.IXC.Tests.Mocks;
using Netplanety.Shared.Exceptions;
using Netplanety.Shared.Interfaces;
using Netplanety.Shared.Models;
using Netplanety.Tests.Common.Mocks;

namespace Netplanety.Integrations.IXC.Tests;

[TestClass]
public sealed class IXCServiceTests
{
	private readonly IOptions<IXCServiceOptions> options;
	private readonly ILogger<IXCService> logger;

	public IXCServiceTests()
	{
		options = Options.Create<IXCServiceOptions>(new IXCServiceOptions
		{
			HttpClientOptions = new HttpClientOptions
			{
				BaseAddress = "https://example.com/api/v1/",

				// Fake base64 string token
				Token = "iIRQBsXVO3YFS4gyZgNq64nyvLKmz9nR7I+/cUVR+7dsx1wUI4eaJ29mkZcQOGOYBRw4iXfoUlF+1iwLJWLhYA=="
			}
		});

		logger = new MockLogger<IXCService>();
	}

	[TestMethod]
	public async Task GetFiberTerminal_ReturnsExpectedResult()
	{
		// Arrange
		var httpMessageHandler = new MockHttpMessageHandler(MockApiResponse.FiberClient_7314);
		var cancellationToken = new CancellationToken();
		var testFiberTerminal = new Ont
		{
			Id = 7314,
			ContractId = 10085,
			ProjectId = "26",
			TransmissorId = "3",
			Name = "12707_BRUNO_ORTEGA_BLANES",
			OpticalTerminationBoxId = "16",
			Port = "13",
			LoginId = "9171",
			PonId = "0/1/5",
			MAC = "4857544376C0E3A5",
			RxSignal = 21.35,
			TxSignal = 25.41,
			Temperature = 52,
			Voltage = 3.32f,
			ProfileId = "1",
			VLAN = "1000"
		};

		// Act
		using var ixcService = new IXCService(logger, new HttpClient(httpMessageHandler), options);
		IOnt? fiberTerminal = await ixcService.GetOntAsync(7314, cancellationToken);

		// Assert
		Assert.IsNotNull(fiberTerminal);
		Assert.AreEqual(testFiberTerminal, fiberTerminal);
	}

	[TestMethod]
	public async Task GetFiberTerminal_ReturnsNull()
	{
		// Arrange
		var httpMessageHandler = new MockHttpMessageHandler(MockApiResponse.FiberClient_Not_Found);
		var cancellationToken = new CancellationToken();

		// Act
		using var ixcService = new IXCService(logger, new HttpClient(httpMessageHandler), options);
		IOnt? fiberTerminal = await ixcService.GetOntAsync(7314, cancellationToken);

		// Assert
		Assert.IsNull(fiberTerminal);
	}

	[TestMethod]
	public async Task GetFiberTerminal_Throws_DeserializationException_On_Broken_Json()
	{
		// Arrange
		var httpMessageHandler = new MockHttpMessageHandler(MockApiResponse.FiberClient_Broken_Json);
		var cancellationToken = new CancellationToken();

		// Act
		using var ixcService = new IXCService(logger, new HttpClient(httpMessageHandler), options);
		Func<Task> GetFiberTerminalAsyncFunc = async () => await ixcService.GetOntAsync(7314, cancellationToken);

		// Assert
		await Assert.ThrowsExceptionAsync<DeserializationException>(GetFiberTerminalAsyncFunc);
	}

	[TestMethod]
	public async Task GetFiberTerminal_Throws_DeserializationException_On_Broken_Response()
	{
		// Arrange
		var httpMessageHandler = new MockHttpMessageHandler(MockApiResponse.FiberClient_Broken_Response);
		var cancellationToken = new CancellationToken();

		// Act
		using var ixcService = new IXCService(logger, new HttpClient(httpMessageHandler), options);
		Func<Task> GetFiberTerminalAsyncFunc = async () => await ixcService.GetOntAsync(7314, cancellationToken);

		// Assert
		await Assert.ThrowsExceptionAsync<DeserializationException>(GetFiberTerminalAsyncFunc);
	}

	[TestMethod]
	public async Task GetFiberTerminal_Throws_DuplicateIdException()
	{
		// Arrange
		var httpMessageHandler = new MockHttpMessageHandler(MockApiResponse.FiberClient_Duplicate_Id);
		var cancellationToken = new CancellationToken();

		// Act
		using var ixcService = new IXCService(logger, new HttpClient(httpMessageHandler), options);
		Func<Task> GetFiberTerminalAsyncFunc = async () => await ixcService.GetOntAsync(7314, cancellationToken);

		// Assert
		await Assert.ThrowsExceptionAsync<DuplicateIdException>(GetFiberTerminalAsyncFunc);
	}
}