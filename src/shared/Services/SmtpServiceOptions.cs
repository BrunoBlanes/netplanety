namespace Netplanety.Shared.Services;

public sealed class SmtpServiceOptions
{
	public string LocalDomain { get; set; }
	public string Host { get; set; }
	public int Port { get; set; }
	public bool UseSSL { get; set; }
	public string User { get; set; }
	public string Password { get; set; }
	public string FromName { get; set; }
	public string FromAddress { get; set; }

	public SmtpServiceOptions()
	{
		Port = 465;
		UseSSL = true;
		Host = string.Empty;
		User = string.Empty;
		Password = string.Empty;
		FromName = string.Empty;
		LocalDomain = "localhost";
		FromAddress = string.Empty;
	}
}
