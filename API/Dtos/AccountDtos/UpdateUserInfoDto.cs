using AsparagusN.Enums;

namespace ProjectP.Dtos.AccountDtos;

public class UpdateUserInfoDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public IFormFile? Image { get; set; }
}