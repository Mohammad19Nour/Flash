using AutoMapper;
using ProjectP.Data.Entities.Identity;
using ProjectP.Dtos.AccountDtos;

namespace ProjectP.Helpers.Profiles;

public class Profiles : Profile
{
    public Profiles()
    {
       CreateMap<double?, double>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);
    }
}