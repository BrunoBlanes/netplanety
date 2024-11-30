using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Netplanety.Shared.Interfaces;

namespace Netplanety.Integrations.IXC.Extensions;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Add support for IXC API integration.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="options">Options to configure the IXC service.</param>
	/// <returns></returns>
	public static IServiceCollection AddIXCSoft(this IServiceCollection services, Action<IXCServiceOptions> options)
	{
		services.AddSingleton<IERPService, IXCService>();
		services.AddHttpClient<IXCService>();
		return services.Configure<IXCServiceOptions>(options);
	}
}
