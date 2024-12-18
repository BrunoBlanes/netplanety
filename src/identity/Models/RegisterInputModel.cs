using System.ComponentModel.DataAnnotations;

using Netplanety.Shared.Attributes;

namespace Netplanety.Identity.Models;

internal class EmailInputModel
{
    [Required, EmailAddress]
    public string Email { get; set; }

    public EmailInputModel()
    {
        Email = string.Empty;
    }

    public EmailInputModel(string email)
    {
        Email = email;
    }
}

internal class PasswordInputModel : EmailInputModel
{
    [Required, Display(Name = "password")]
    [StringLength(100, ErrorMessage = "Your {0} must be at least {2} and at max {1} characters long", MinimumLength = 8)]
    public string Password { get; set; }

    public PasswordInputModel(EmailInputModel model)
    {
        Email = model.Email;
        Password = string.Empty;
    }

    public PasswordInputModel(PasswordInputModel model)
    {
        Email = model.Email;
        Password = model.Password;
    }
}

internal class CpfInputModel : PasswordInputModel
{
    [Required, CPF]
    public string CPF { get; set; }

    public CpfInputModel(PasswordInputModel model) : base(model)
    {
        CPF = string.Empty;
    }
}
