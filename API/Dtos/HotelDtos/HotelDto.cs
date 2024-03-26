using System.ComponentModel.DataAnnotations;
using ProjectP.Data.Entities;
using ProjectP.Dtos.OfferDtos;

namespace ProjectP.Dtos.HotelDtos;

public class HotelDto
{
    public int Id { get; set; }
    [Required] [MaxLength(70)]  public string ArabicName { get; set; }
    [Required] [MaxLength(70)]  public string EnglishName { get; set; }
    public string Email { get; set; }
    public int Stars { get; set; }
    public int Reviews { get; set; }
    
    [Required] [MaxLength(400)] public string ArabicDescription { get; set; }
    [Required] [MaxLength(400)] public string EnglishDescription { get; set; }
    public double MinPrice { get; set; }
     public double MaxPrice { get; set; }
    public ICollection<PhotoDto> Photos { get; set; }
    public LocationDtoss.LocationDto Location { get; set; }
    public OfferDto Offer { get; set; }
}