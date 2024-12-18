namespace Netplanety.Integrations.IXC.Extensions.Http;

/// <summary>
/// Options to configure the <see cref="HttpClientExtensions"/>.
/// </summary>
public sealed class HttpClientOptions
{
    /// <summary>
    /// The main API base address.
    /// </summary>
    /// <remarks>Usually <see href="https://example.com/webservice/v1/"/></remarks>
    public string BaseAddress { get; set; }

    /// <summary>
    /// The user API token provided by IXC.
    /// </summary>
    public string Token { get; set; }

    internal HttpClientOptions()
    {
        Token = string.Empty;
        BaseAddress = string.Empty;
    }
}
