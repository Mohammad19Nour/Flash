using ProjectP.Data.Entities.Identity;

namespace ProjectP.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}