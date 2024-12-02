using MailKit.Net.Smtp;

using Microsoft.Extensions.DependencyInjection;

using Netplanety.Shared.Services;

namespace Netplanety.Shared.Extensions;

public static class ServiceCollectionExtensions
{
	public static ISmtpServiceCollection AddSmtpService(this IServiceCollection services, Action<SmtpServiceOptions> options)
	{
		services.Configure<SmtpServiceOptions>(options);
		services.AddSingleton<ISmtpClient, SmtpClient>();
		services.AddSingleton<ISmtpService, SmtpService>();
		return new SmtpServiceCollection(services);
	}
}

public interface ISmtpServiceCollection;

public sealed class SmtpServiceCollection : ISmtpServiceCollection
{
	public IServiceCollection Services { get; init; }

	internal SmtpServiceCollection(IServiceCollection services)
	{
		Services = services;
	}
}