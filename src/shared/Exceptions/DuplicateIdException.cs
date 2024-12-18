namespace Netplanety.Shared.Exceptions;

public sealed class DuplicateIdException : Exception
{
    public int Id { get; init; }

    public DuplicateIdException(int id)
    {
        Id = id;
    }
}
