using AutoMapper;
using ProjectP.Data.Entities;
using ProjectP.Dtos.HotelDtos;
using ProjectP.Dtos.TouristPlacesDtos;

namespace ProjectP.Helpers.Profiles;

public class TouristPlacesProfile : Profile
{
    public TouristPlacesProfile()
    {
        CreateMap<NewTouristPlacesDto, TouristPlace>();
        CreateMap<UpdateTouristPlacesDto, TouristPlace>()
            .ForAllMembers(dest=>dest.Condition((src,d,mem)=>mem != null));
        CreateMap<TouristPlace, TouristPlacesDto>();
    }
}