using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Netplanety.Tests.Common.Mocks;

public sealed class MockHttpMessageHandler : HttpMessageHandler
{
	private readonly string mockContent;

	public MockHttpMessageHandler(string mockContent)
	{
		this.mockContent = mockContent;
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return Task.FromResult(new HttpResponseMessage
		{
			Content = new MockHttpContent(Encoding.UTF8.GetBytes(mockContent))
		});
	}
}