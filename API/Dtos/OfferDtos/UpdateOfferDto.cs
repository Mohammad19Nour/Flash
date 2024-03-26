using System.ComponentModel.DataAnnotations;

namespace ProjectP.Dtos.OfferDtos;

public class UpdateOfferDto
{
     public DateTime? StartAt { get; set; }
     public DateTime? EndAt { get; set; }
    public double Discount { get; set; }
}