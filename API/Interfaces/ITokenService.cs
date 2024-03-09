using ProjectP.Data.Entities.Identity;
using ProjectP.Entities;

namespace ProjectP.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}