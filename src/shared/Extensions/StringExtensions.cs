namespace Netplanety.Shared.Extensions;

public static class StringExtensions
{
    public static string Capitalize(this string text)
    {
        ReadOnlySpan<char> value = text.ToLowerInvariant();
        return string.Concat(value[0].ToString().ToUpperInvariant(), value[1..]);
    }
}
