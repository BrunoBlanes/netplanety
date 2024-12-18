namespace Netplanety.Shared.Exceptions;

public sealed class DeserializationException : Exception
{
    public string Content { get; init; }

    public DeserializationException(string content)
    {
        Content = content;
    }
}
