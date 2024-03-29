namespace ProjectP.Dtos;

public class HotelFilterParameters
{
    
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public int? MinStars { get; set; }
    public int? MaxStars { get; set; }
  //  public int? Beds { get; set; }
    public string? RoomType { get; set; }

}