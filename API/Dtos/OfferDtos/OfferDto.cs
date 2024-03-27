using ProjectP.Dtos.HotelDtos;

namespace ProjectP.Dtos.OfferDtos;

public class OfferDto : OfferForHotelDto
{
    
    public HotelForOfferDto Hotel { get; set; }
}