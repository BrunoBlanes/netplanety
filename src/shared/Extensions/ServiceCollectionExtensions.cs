using MailKit.Net.Smtp;

using Microsoft.Extensions.DependencyInjection;

using Netplanety.Shared.Services.Smtp;

namespace Netplanety.Shared.Extensions;

/// <summary>
/// A collection of extensions to the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures an SMTP service.
    /// </summary>
    /// <param name="services">A collection of services for the application to compose.
    /// This is useful for adding user provided or framework provided services.</param>
    /// <param name="options">Options to configure the SMTP client with.</param>
    /// <returns>An <see cref="SmtpBuilder"/> for creating and configuring the SMTP system.</returns>
    public static SmtpBuilder AddSmtpClient(this IServiceCollection services, Action<SmtpServiceOptions> options)
    {
        services.Configure<SmtpServiceOptions>(options);
        services.AddSingleton<ISmtpClient, SmtpClient>();
        services.AddSingleton<ISmtpService, SmtpService>();
        return new SmtpBuilder(services);
    }
}
