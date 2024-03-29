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
        CreateMap<UpdateTouristPlacesDto, TouristPlace>();
        CreateMap<TouristPlace, TouristPlacesDto>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location.City))
            ; // .ForMember(dest => dest.ima, opt => opt.MapFrom(src => src.Location.Longitude))
        // .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Latitude));
    }
}