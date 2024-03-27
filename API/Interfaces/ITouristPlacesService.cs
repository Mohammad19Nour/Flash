

using ProjectP.Dtos.TouristPlacesDtos;

namespace ProjectP.Interfaces;

public interface ITouristPlacesService
{
    Task<List<TouristPlacesDto>> GetAllPlacesAsync();
    Task<TouristPlacesDto?> GetPlaceByIdAsync(int id);
    Task<(TouristPlacesDto? location, string message)> AddPlaceAsync(NewTouristPlacesDto places);
    Task<(TouristPlacesDto? location, string message)> UpdatePlaceAsync(int id,UpdateTouristPlacesDto place);
    Task<(bool succeed, string message)> DeletePlaceByIdAsync(int id);
}