using ProjectP.Dtos.LocationDtoss;

namespace ProjectP.Dtos.HotelDtos;

public class HotelForOfferDto
{
    public int Id { get; set; }
      public string ArabicName { get; set; }
      public string EnglishName { get; set; }
    public string Email { get; set; }
    public int Stars { get; set; }
    public int Reviews { get; set; }
    
    public string ArabicDescription { get; set; }
    public string EnglishDescription { get; set; }
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public ICollection<PhotoDto> Photos { get; set; }
    public LocationDto Location { get; set; }
    public bool IsFavorite { get; set; } = false;
}