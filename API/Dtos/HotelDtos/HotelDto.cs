using ProjectP.Data.Entities;

namespace ProjectP.Dtos.HotelDtos;

public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Stars { get; set; }
    public int Reviews { get; set; }
    
    public string Description { get; set; }
    public double MinPrice { get; set; }
     public double MaxPrice { get; set; }
    public ICollection<Photo> Photos { get; set; }
    public LocationDtoss.LocationDto Location { get; set; }
}