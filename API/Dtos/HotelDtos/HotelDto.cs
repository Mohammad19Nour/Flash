using System.ComponentModel.DataAnnotations;
using ProjectP.Data.Entities;
using ProjectP.Dtos.OfferDtos;

namespace ProjectP.Dtos.HotelDtos;

public class HotelDto : HotelForOfferDto
{
    public OfferForHotelDto Offer { get; set; }
}