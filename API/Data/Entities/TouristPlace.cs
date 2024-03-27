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

    public Location Location { get; set; }

}