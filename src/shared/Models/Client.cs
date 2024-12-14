using Netplanety.Shared.Interfaces;

namespace Netplanety.Shared.Models;

public struct Client : IClient
{
	public int Id { get; set; }
	public string CPF { get; set; }
	public bool IsActive { get; set; }
	public string Name { get; set; }
	public DateTime Birthday { get; set; }

	public Client()
	{
		CPF = string.Empty;
		Name = string.Empty;
	}
}
