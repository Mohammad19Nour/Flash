using System.ComponentModel.DataAnnotations;

namespace ProjectP.Data.Entities;

public class TouristPlace
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string ArabicName { get; set; }

    [MaxLength(100)]
    public string EnglishName { get; set; }

    [MaxLength(500)]
    public string ArabicDescription { get; set; }

    [MaxLength (500)]
    public string EnglishDescription { get; set; }

    public double Longitude { get; set; } = 0;
    public double Latitude { get; set; } = 0;
    public string City { get; set; }
    public string CityEN { get; set; }
    public List<Photo> Photos { get; set; } = new List<Photo>();

}