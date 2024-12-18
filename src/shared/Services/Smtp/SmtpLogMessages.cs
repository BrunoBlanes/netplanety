namespace Netplanety.Shared.Services.Smtp;

/// <summary>
/// Log message templates for the <see cref="SmtpService"/>.
/// </summary>
internal readonly struct SmtpLogMessages
{
    /// <summary>
    /// The log template for when the connection is established.
    /// </summary>
    public const string Connected = "SMTP connection successfully established with {host}:{port}";

    /// <summary>
    /// The log template for when the user is authenticated.
    /// </summary>
    public const string Authenticated = "SMTP user {user} authentication successful";
}
