using Microsoft.AspNetCore.Authorization;

using Netplanety.Shared.Identity;

namespace Netplanety.Shared.Extensions;

/// <summary>
/// Extensions to the <see cref="AuthorizationPolicyBuilder"/> class.
/// </summary>
public static class AuthorizationPolicyBuilderExtensions
{
	/// <summary>
	/// Adds a <see cref="Microsoft.AspNetCore.Authorization.Infrastructure.ClaimsAuthorizationRequirement"/> to the
	/// current intance which requires that the current user has the <c>scope</c> claim and that the claim value must
	/// be one of the <see cref="IdentityScopes"/> values.
	/// </summary>
	/// <param name="policy">The <see cref="AuthorizationPolicyBuilder"/> instance.</param>
	/// <param name="flags">The claim value as one or more <see cref="IdentityScopes"/> flags.</param>
	/// <returns></returns>
	public static AuthorizationPolicyBuilder RequireScope(this AuthorizationPolicyBuilder policy, IdentityScopes flags)
	{
		return policy.RequireClaim("scope", flags.ToStringArray());
	}
}
