namespace Netplanety.Shared.Services.Smtp;

/// <summary>
/// Provides options to configure an <see cref="ISmtpService"/>.
/// </summary>
public sealed class SmtpServiceOptions
{
	/// <summary>
	/// The local domain of the server. Defaults to <c>localhost</c>.
	/// </summary>
	public string LocalDomain { get; set; }

	/// <summary>
	/// The SMTP server address.
	/// </summary>
	public string Host { get; set; }

	/// <summary>
	/// The SMTP server port. Defaults to <c>465</c>.
	/// </summary>
	/// <remarks>The value must be between <c>0</c> and <c>65535</c>.</remarks>
	public int Port { get; set; }

	/// <summary>
	/// Gets or sets a flag that indicates if SSL encryption should be used. Defaults to <c>true</c>.
	/// </summary>
	public bool UseSSL { get; set; }

	/// <summary>
	/// Gets or sets a flag that indicates whether the <c>REQUIRETLS</c> extension should be used. Defaults to <c>true</c>.
	/// </summary>
	public bool RequireTLS { get; set; }

	/// <summary>
	/// Gets or sets a flag that indicates whether the server certificate should be checked for revocation.
	/// </summary>
	public bool CheckCertificateRevocation { get; set; }

	/// <summary>
	/// The username to authenticate with the SMTP server.
	/// </summary>
	public string User { get; set; }

	/// <summary>
	/// The password to authenticate with the SMTP server.
	/// </summary>
	public string Password { get; set; }

	/// <summary>
	/// The sender name.
	/// </summary>
	public string FromName { get; set; }

	/// <summary>
	/// The sender address.
	/// </summary>
	public string FromAddress { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="SmtpServiceOptions"/> class.
	/// </summary>
	public SmtpServiceOptions()
	{
		Port = 465;
		UseSSL = true;
		RequireTLS = true;
		Host = string.Empty;
		User = string.Empty;
		Password = string.Empty;
		FromName = string.Empty;
		LocalDomain = "localhost";
		FromAddress = string.Empty;
		CheckCertificateRevocation = true;
	}
}
