using System.Net;

namespace Netplanety.Tests.Common.Mocks;

public sealed class MockHttpContent : HttpContent
{
    private readonly byte[] _buffer;

    public MockHttpContent(byte[] buffer)
    {
        _buffer = buffer;
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        await stream.WriteAsync(_buffer);
    }

    protected override bool TryComputeLength(out long length)
    {
        length = _buffer.Length;
        return true;
    }
}
