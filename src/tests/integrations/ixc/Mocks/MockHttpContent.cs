using System.Net;

namespace Netplanety.Integrations.IXC.Tests.Mocks;

internal sealed class MockHttpContent : HttpContent
{
	private readonly byte[] buffer;

	public MockHttpContent(byte[] buffer)
	{
		this.buffer = buffer;
	}

	protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
	{
		await stream.WriteAsync(buffer, 0, buffer.Length);
	}

	protected override bool TryComputeLength(out long length)
	{
		length = buffer.Length;
		return true;
	}
}
