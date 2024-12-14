using Microsoft.AspNetCore.Identity;

namespace Netplanety.Shared.Models;

public class ApplicationUser : IdentityUser<Guid>
{
	public string CPF { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string FullName { get => $"{FirstName} {LastName}"; }

	public ApplicationUser() : base()
	{
		CPF = string.Empty;
		LastName = string.Empty;
		FirstName = string.Empty;
	}
}

