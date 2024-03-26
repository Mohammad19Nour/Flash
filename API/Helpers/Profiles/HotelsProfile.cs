using AutoMapper;
using ProjectP.Data.Entities;
using ProjectP.Dtos;
using ProjectP.Dtos.HotelDtos;
using ProjectP.Dtos.LocationDtoss;

namespace ProjectP.Helpers.Profiles;

public class HotelsProfile : Profile
{
    public HotelsProfile()
    {
        CreateMap<Hotel, HotelDto>();
        CreateMap<UpdateHotelDto, Hotel>().ForAllMembers(x =>
            x.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<NewHotelDto, Hotel>()
            .ForMember(dest => dest.Photos, opt => opt.Ignore());
        
        CreateMap<Location, LocationDto>();
        CreateMap<UpdateLocationDto,Location>().ForAllMembers(x =>
            x.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<NewLocationDto, Location>();

    }
}