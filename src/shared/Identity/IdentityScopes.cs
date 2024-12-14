using System.Collections.ObjectModel;
using System.Text;

namespace Netplanety.Shared.Identity;

/// <summary>
/// Extensions to the <see cref="IdentityScopes"/> enum.
/// </summary>
public static class IdentityScopesExtensions
{
	/// <summary>
	/// Converts the set <paramref name="flags"/> into a <see cref="string"/> array of scopes.
	/// </summary>
	/// <param name="flags">The <see cref="IdentityScopes"/> flags.</param>
	/// <returns>A <see cref="string"/> array of scopes.</returns>
	public static string[] ToStringArray(this IdentityScopes flags)
	{
		var scopes = Enumerable.Empty<string>();

		// Loop through all the available flags
		foreach (IdentityScopes scope in Enum.GetValues(typeof(IdentityScopes)))
		{
			// Do a bitwise and operation to
			// check if the current flag was set
			if ((flags & scope) == scope)
			{
				// Appends the lowercase flag name separated by a space
				scopes = scopes.Append(scope.ToString().ToLowerInvariant());
			}
		}

		// Trims the end space
		// and returns the string
		return scopes.ToArray();
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