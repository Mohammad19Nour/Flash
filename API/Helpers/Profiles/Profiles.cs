using AutoMapper;
using ProjectP.Data.Entities.Identity;
using ProjectP.Dtos.AccountDtos;
using ProjectP.Entities;

namespace ProjectP.Helpers.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<AppUser,AccountDto>();
        CreateMap<RegisterDto, AppUser>();
    }
}