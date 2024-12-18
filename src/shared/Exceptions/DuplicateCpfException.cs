namespace Netplanety.Shared.Exceptions;

public sealed class DuplicateCpfException : Exception
{
    public string CPF { get; init; }

    public DuplicateCpfException(string cpf)
    {
        CPF = cpf;
    }
}