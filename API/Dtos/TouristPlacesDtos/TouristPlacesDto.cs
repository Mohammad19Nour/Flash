using ProjectP.Data.Entities;
using ProjectP.Dtos.HotelDtos;

namespace ProjectP.Dtos.TouristPlacesDtos;

public class TouristPlacesDto
{
    public int Id { get; set; }
    
    public string ArabicName { get; set; }
    
    public string EnglishName { get; set; }
    
    public string ArabicDescription { get; set; }
    
    public string EnglishDescription { get; set; }
    public string City { get; set; }

    public List<PhotoDto> Photos { get; set; }
   // public double Longitude { get; set; }
   // public double Latitude { get; set; }

}