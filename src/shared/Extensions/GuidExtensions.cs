namespace Netplanety.Shared.Extensions;

/// <summary>
/// Extensions to the <see cref="Guid"/> struct.
/// </summary>
public readonly struct GuidExtensions
{
    /// <summary>
    /// Creates a deterministic <see cref="Guid"/> using the specified <paramref name="seed"/>.
    /// </summary>
    /// <param name="seed">A <paramref name="seed"/> value used for the underlying <see cref="Random"/> bytes generator.</param>
    /// <returns>The deterministic <see cref="Guid"/>.</returns>
    public static Guid NewGuid(int seed)
    {
        var random = new Random(seed);
        byte[] randomBytes = new byte[16];
        random.NextBytes(randomBytes);
        return new Guid(randomBytes);
    }
}
