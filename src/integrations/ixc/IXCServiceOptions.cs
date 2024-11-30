using Netplanety.Integrations.IXC.Extensions.Http;

namespace Netplanety.Integrations.IXC;

/// <summary>
/// Options to configure the <see cref="IXCService"/> with.
/// </summary>
public sealed class IXCServiceOptions
{
	public HttpClientOptions HttpClientOptions { get; set; }

	internal IXCServiceOptions()
	{
		HttpClientOptions = new HttpClientOptions();
	}
}
