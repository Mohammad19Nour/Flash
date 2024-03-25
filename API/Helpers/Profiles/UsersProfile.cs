using ProjectP.Data.Entities.Identity;
using ProjectP.Dtos.AccountDtos;

namespace ProjectP.Helpers.Profiles;

public class UsersProfile : Profiles
{
    public UsersProfile()
    {
        CreateMap<AppUser,AccountDto>();
        CreateMap<RegisterDto, AppUser>();
        CreateMap<AppUser, UserInfoDto>();
        CreateMap<UpdateUserInfoDto,AppUser>().ForAllMembers(dest=>dest.Condition((src,d,m)=>m != null));
    }
}