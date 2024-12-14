namespace Netplanety.Integrations.IXC.Extensions.Http;

/// <summary>
/// All the available API endpoints.
/// </summary>
internal readonly struct QueryEndpoints
{
	/// <summary>
	/// The <see cref="Models.IXCOnt"/> endpoint.
	/// </summary>
	internal const string FiberClient = "radpop_radio_cliente_fibra";

	/// <summary>
	/// The <see cref="Models.IXCClient"/> endpoint.
	/// </summary>
	internal const string Client = "cliente";
}