using Microsoft.Extensions.DependencyInjection;

namespace Netplanety.Shared.Services.Smtp;

/// <summary>
/// Helper functions for configuring the SMTP services.
/// </summary>
public class SmtpBuilder
{
    /// <summary>
    /// A collection of services for the application to compose.
    /// This is useful for adding user provided or framework provided services.
    /// </summary>
    protected internal IServiceCollection Services { get; init; }

    /// <summary>
    /// Creates a new instance of the <see cref="SmtpBuilder"/> class.
    /// </summary>
    /// <param name="services">A collection of services for the application to compose.
    /// This is useful for adding user provided or framework provided services.</param>
    public SmtpBuilder(IServiceCollection services)
    {
        Services = services;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SmtpBuilder"/> class.
    /// </summary>
    /// <param name="builder">Helper functions for configuring the SMTP services.</param>
    public SmtpBuilder(SmtpBuilder builder)
    {
        Services = builder.Services;
    }
}