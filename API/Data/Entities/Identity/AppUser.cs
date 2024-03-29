using Microsoft.AspNetCore.Identity;
using ProjectP.Enums;
using ProjectP.Extensions;

namespace ProjectP.Data.Entities.Identity;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }  
    public ICollection<AppUserRole> UserRoles { get; set; }
    public string? PictureUrl { get; set; }
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string CountryPrifix { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    public DateTime Birthday { get; set; } = DateTime.Today;
    public int GetAge()
    {
        return Birthday.NumberOfYears();
    }
}