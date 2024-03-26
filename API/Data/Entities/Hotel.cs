using System.ComponentModel.DataAnnotations;

namespace ProjectP.Data.Entities;

public class Hotel
{
    public int Id { get; set; }
    [Required] [MaxLength(100)] public string ArabicName { get; set; }
    [Required] [MaxLength(100)] public string EnglishName { get; set; }
    [Required] [MaxLength(100)] public string Email { get; set; }
    public int Stars { get; set; }
    public int Reviews { get; set; }
    
    [Required] [MaxLength(400)] public string ArabicDescription { get; set; }
    [Required] [MaxLength(400)] public string EnglishDescription { get; set; }
    [Required] [Range(0, double.MaxValue)] public double MinPrice { get; set; }
    [Required] [Range(0, double.MaxValue)] public double MaxPrice { get; set; }
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
  //  public ICollection<RoomType> RoomTypes { get; set; }
    public Location Location { get; set; }
    public int LocationId { get; set; }

    public Offer? Offer { get; set; }
    public int? OfferId { get; set; }

}
