using System.ComponentModel.DataAnnotations;
using ProjectP.Dtos.LocationDtoss;

namespace ProjectP.Dtos.HotelDtos;

public class NewHotelDto
{
    [Required] [MaxLength(100)] public string ArabicName { get; set; }
    [Required] [MaxLength(100)] public string EnglishName { get; set; }
   [EmailAddress] [Required] [MaxLength(100)] public string Email { get; set; }
    public int Stars { get; set; }
    public int Reviews { get; set; }
    
    [Required] [MaxLength(400)] public string ArabicDescription { get; set; }
    [Required] [MaxLength(400)] public string EnglishDescription { get; set; }
    [Required] [Range(0, double.MaxValue)] public double MinPrice { get; set; }
    [Required] [Range(0, double.MaxValue)] public double MaxPrice { get; set; }
   [Required] [MinLength(1,ErrorMessage = "should add at least one photo")] public ICollection<IFormFile> Photos { get; set; }
    public NewLocationDto Location { get; set; }
}