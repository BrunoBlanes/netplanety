namespace Netplanety.Shared.Interfaces;

public interface IClient
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string CPF { get; }
}
