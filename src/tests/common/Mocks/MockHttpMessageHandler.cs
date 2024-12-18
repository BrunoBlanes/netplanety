using System.Text;

namespace Netplanety.Tests.Common.Mocks;

public sealed class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly string _mockContent;

    public MockHttpMessageHandler(string mockContent)
    {
        _mockContent = mockContent;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpResponseMessage
        {
            Content = new MockHttpContent(Encoding.UTF8.GetBytes(_mockContent))
        });
    }
}
