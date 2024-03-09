namespace ProjectP.Dtos.AccountDtos;

public class AccountDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    
    public DateTime RegistrationDate { get; set; }
    
    public string PictureUrl { get; set; }
    public string Gender { get; set; }
    
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Birthday { get; set; }

}