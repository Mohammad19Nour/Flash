using System.ComponentModel.DataAnnotations;
using ProjectP.Enums;

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

    [Required(ErrorMessage = "First name required")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Last name required")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Phone number required")]
    public string PhoneNumber { get; set; }
    public IFormFile? Image { get; set; }
}