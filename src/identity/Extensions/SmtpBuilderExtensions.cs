using Microsoft.AspNetCore.Identity;

using Netplanety.Identity.Services;
using Netplanety.Shared.Models;
using Netplanety.Shared.Services.Smtp;

namespace Netplanety.Identity.Extensions;

/// <summary>
/// A collection of extensions to the <see cref="SmtpBuilder"/> class.
/// </summary>
internal static class SmtpBuilderExtensions
{
    /// <summary>
    /// Adds ASP.NETCore Identity support through the implementation of the <see cref="IEmailSender{TUser}"/> interface.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns>An <see cref="IIdentitySmtpBuilder"/>.</returns>
    internal static IIdentitySmtpBuilder AddIdentitySupport(this SmtpBuilder builder)
    {
        return new IdentitySmtpBuilder(builder);
    }
}

/// <summary>
/// Helper functions for configuring Identity SMTP services.
/// </summary>
internal interface IIdentitySmtpBuilder;

/// <summary>
/// Helper functions for configuring Identity SMTP services.
/// </summary>
internal sealed class IdentitySmtpBuilder : SmtpBuilder, IIdentitySmtpBuilder
{
    /// <summary>
    /// Creates a new instance of the <see cref="IdentitySmtpBuilder"/> class.
    /// </summary>
    /// <param name="builder">Helper functions for configuring the SMTP services.</param>
    internal IdentitySmtpBuilder(SmtpBuilder builder) : base(builder)
    {
        Services.AddSingleton<IEmailSender<ApplicationUser>, IdentitySmtpService>();
    }
}
