

using ProjectP.Dtos.TouristPlacesDtos;

namespace ProjectP.Interfaces;

public interface ITouristPlacesService
{
    Task< List<Dictionary<string,object>>> GetAllPlacesAsync();
    Task<TouristPlacesDto?> GetPlaceByIdAsync(int id);
    Task<(TouristPlacesDto? location, string message)> AddPlaceAsync(NewTouristPlacesDto places);
    Task<(TouristPlacesDto? location, string message)> UpdatePlaceAsync(int id,UpdateTouristPlacesDto place);
    Task<(bool succeed, string message)> DeletePlaceByIdAsync(int id);
    Task<(bool succeed, string message)> AddPhotosAsync(int placeId,ICollection<IFormFile> images);
    Task<(bool succeed, string message)> DeletePhotoAsync(int placeId,int imageId);

}