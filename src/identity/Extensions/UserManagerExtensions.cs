using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Netplanety.Shared.Models;

namespace Netplanety.Identity.Extensions;

public static class UserManagerExtensions
{
    public static Task<ApplicationUser?> GetUserByCpfAsync(this UserManager<ApplicationUser> userManager, string cpf, CancellationToken cancellationToken)
    {
        return userManager.Users.FirstOrDefaultAsync(user => user.CPF == cpf, cancellationToken);
    }
}
