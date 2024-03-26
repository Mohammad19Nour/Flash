using System.ComponentModel.DataAnnotations;

namespace ProjectP.Dtos.OfferDtos;

public class NewOfferDto
{
  [Required (ErrorMessage = "Hotel id is required")]
  public int HotelId { get; set; }
  [Required(ErrorMessage = "Start date of the offer is required")]  public DateTime StartAt { get; set; }
  [Required(ErrorMessage = "End date of the offer is required")]  public DateTime EndAt { get; set; }
  public double Discount { get; set; }
}