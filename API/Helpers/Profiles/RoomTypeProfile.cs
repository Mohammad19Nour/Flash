using AutoMapper;
using ProjectP.Data.Entities;
using ProjectP.Dtos.HotelDtos;

namespace ProjectP.Helpers.Profiles;

public class RoomTypeProfile : Profile
{
    public RoomTypeProfile()
    {
        CreateMap<RoomType, RoomTypeDto>();
    }
}