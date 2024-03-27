using ProjectP.Dtos.HotelDtos;

namespace ProjectP.Interfaces;

public interface IFavoriteService
{
    Task<(bool succeed,string message)> ToggleHotelInFavorite(int hotelId, string userEmail);
   Task<List<HotelDto>> GetFavoriteHotels(string userEmail);
}