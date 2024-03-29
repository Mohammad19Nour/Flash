namespace ProjectP.Data.Entities;

public class RoomType
{
    public int Id { get; set; }
    public string ArabicName { get; set; }
    public string EnglishName { get; set; }

    public ICollection<HotelRoomType> HotelRoomTypes { get; set; } = new List<HotelRoomType>();
}