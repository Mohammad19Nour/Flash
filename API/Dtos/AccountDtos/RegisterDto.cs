using System.ComponentModel.DataAnnotations;
using AsparagusN.Enums;

namespace ProjectP.Dtos.AccountDtos;

public class RegisterDto
{
    [Required(ErrorMessage = "Email required")]
  [EmailAddress]  public string Email { get; set; }

    [Required(ErrorMessage = "Password required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "co pass required")]
    public string ConfirmedPassword { get; set; }

    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Full name required")]
    public string FullName { get; set; }
    
    [Required(ErrorMessage = "birthday required")]
    public DateTime Birthday { get; set; }
    public IFormFile Image { get; set; }
}