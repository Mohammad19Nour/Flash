using System.ComponentModel.DataAnnotations;
using ProjectP.Enums;

namespace ProjectP.Dtos.AccountDtos;

public class UpdateUserInfoDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public Gender? Gender { get; set; }
  [EmailAddress]  public string? Email { get; set; }
    public IFormFile? Image { get; set; }
    public string? CountryPrifix { get; set; }
}