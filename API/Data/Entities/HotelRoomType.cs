namespace ProjectP.Data.Entities;

public class HotelRoomType
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }

    public int RoomTypeId { get; set; }
    public RoomType RoomType { get; set; }
}