namespace Netplanety.Shared.Identity;

/// <summary>
/// Extensions to the <see cref="IdentityScopes"/> enum.
/// </summary>
public static class IdentityScopesExtensions
{
    /// <summary>
    /// Converts the <paramref name="flags"/> to a <see cref="ICollection{string}"/>.
    /// </summary>
    /// <param name="flags">The <see cref="IdentityScopes"/> flags.</param>
    /// <returns>An <see cref="ICollection{string}"/> with the parsed <paramref name="flags"/> as lower case invariant.</returns>
    public static ICollection<string> ToStringCollection(this IdentityScopes flags)
    {
        ICollection<string> scopes = [];

        // Loop through all the available flags
        foreach (IdentityScopes scope in Enum.GetValues<IdentityScopes>())
        {
            // Do a bitwise and operation to
            // check if the current flag was set
            if ((flags & scope) == scope)
            {
                // Appends the lowercase flag name separated by a space
                scopes.Add(scope.ToString().ToLowerInvariant());
            }
        }

        // Trims the end space
        // and returns the string
        return [.. scopes];
    }

    /// <summary>
    /// Converts the <paramref name="scope"/> value to its lower case <see cref="string"/> representation using the invariant culture.
    /// </summary>
    /// <param name="scope">The <see cref="IdentityScopes"/> scope.</param>
    /// <returns>The lower case <see cref="string"/> representation of the <paramref name="scope"/>.</returns>
    public static string ToLowerInvariantString(this IdentityScopes scope)
    {
        return scope.ToString().ToLowerInvariant();
    }
}

/// <summary>
/// The identity scopes.
/// </summary>
[Flags]
public enum IdentityScopes
{
    /// <summary>
    /// Allows read access to the resource.
    /// </summary>
    Read,

    /// <summary>
    /// Allows write access to the resource.
    /// </summary>
    Write,

    /// <summary>
    /// Allows delete access to the resource.
    /// </summary>
    Delete
}
