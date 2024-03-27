using System.ComponentModel.DataAnnotations;

namespace ProjectP.Dtos.TouristPlacesDtos;

public class NewTouristPlacesDto
{
    [MaxLength(100)]
    public string ArabicName { get; set; }

    [MaxLength (100)]
    public string EnglishName { get; set; }

    [MaxLength(500)]
    public string ArabicDescription { get; set; }

    [MaxLength (500)]
    public string EnglishDescription { get; set; }
    
    [Required] [MaxLength(100)] public string City { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}