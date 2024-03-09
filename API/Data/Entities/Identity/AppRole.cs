using Microsoft.AspNetCore.Identity;

namespace ProjectP.Data.Entities.Identity;

public class AppRole : IdentityRole<int>
{
    public ICollection<AppUserRole> UserRoles { get; set; }
}