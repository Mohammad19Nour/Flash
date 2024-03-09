using AsparagusN.Enums;
using Microsoft.AspNetCore.Identity;

namespace ProjectP.Data.Entities.Identity;

public class AppUser : IdentityUser<int>
{
    public string Email { get; set; }  
    public ICollection<AppUserRole> UserRoles { get; set; }
    public string PictureUrl { get; set; }
    public Gender Gender { get; set; }
}