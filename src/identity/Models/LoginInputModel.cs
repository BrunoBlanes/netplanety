using System.ComponentModel.DataAnnotations;

namespace Netplanety.Identity.Models;

internal sealed class LoginInputModel
{
    [Required]
    [EmailAddress]
    internal string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    internal string Password { get; set; }

    [Display(Name = "Remember me?")]
    internal bool RememberMe { get; set; }

    internal LoginInputModel()
    {
        Email = string.Empty;
        Password = string.Empty;
    }
}