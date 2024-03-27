using ProjectP.Data.Entities.Identity;

namespace ProjectP.Data.Entities;

public class UserHotel
{
    public int UserId { get; set; }
    public int HotelId { get; set; }

    public AppUser User { get; set; }
    public Hotel Hotel { get; set; }
}