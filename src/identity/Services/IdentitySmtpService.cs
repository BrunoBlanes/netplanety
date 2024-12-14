using Microsoft.AspNetCore.Identity;

using Netplanety.Shared.Models;
using Netplanety.Shared.Services.Smtp;

namespace Netplanety.Identity.Services;

public sealed class IdentitySmtpService : IEmailSender<ApplicationUser>, IAsyncDisposable
{
	private readonly ISmtpService smtpService;

	public IdentitySmtpService(ISmtpService smtpService, ILogger<IdentitySmtpService> logger)
	{
		this.smtpService = smtpService;
	}

	public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
	{
		return smtpService.SendAsync(
			user.FirstName,
			email,
			"Confirmation link",
			confirmationLink,
			default);
	}

	public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
	{
		return smtpService.SendAsync(
			user.FirstName,
			email,
			"Reset code",
			resetCode,
			default);
	}

	public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
	{
		return smtpService.SendAsync(
			user.FirstName,
			email,
			"Reset link",
			resetLink,
			default);
	}

	public async ValueTask DisposeAsync()
	{
		await smtpService.DisposeAsync();
	}
}
