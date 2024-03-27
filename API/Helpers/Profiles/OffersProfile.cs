using AutoMapper;
using ProjectP.Data.Entities;
using ProjectP.Dtos.OfferDtos;

namespace ProjectP.Helpers.Profiles;

public class OffersProfile : Profile
{
    public OffersProfile()
    {
        CreateMap<Offer, OfferDto>();
        
        CreateMap<UpdateOfferDto, Offer>().ForAllMembers(x =>
            x.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<NewOfferDto, Offer>();

        CreateMap<Offer, OfferForHotelDto>();
    }
}