using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data.Entities;
using ProjectP.Dtos.TouristPlacesDtos;
using ProjectP.Interfaces;

namespace ProjectP.Services;

public class TouristPlacesService : ITouristPlacesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public TouristPlacesService(IUnitOfWork unitOfWork, IMapper mapper,IMediaService mediaService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    public async Task< List<Dictionary<string,object>>> GetAllPlacesAsync()
    {
        var dbPlaces = await _unitOfWork.Repository<TouristPlace>().GetQueryable()
            .Include(p=>p.Photos)
            .ToListAsync();

        var grouped = dbPlaces.GroupBy(p => new { p.CityEN, p.City })
            .Select(group => new Dictionary<string, object>
            {
                {"city" , group.Key.City},
                { "cityEN" , group.Key.CityEN },
                { "touristplaces" , group.Select(c=>_mapper.Map<TouristPlacesDto>(c)).ToList() }
            }).ToList();
        return grouped;
    }

    public async Task<TouristPlacesDto?> GetPlaceByIdAsync(int id)
    {
        var dbPlace = await _unitOfWork.Repository<TouristPlace>().GetQueryable()
            .Include(p=>p.Photos)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (dbPlace == null)
            return null;

        return _mapper.Map<TouristPlacesDto>(dbPlace);
    }

    public async Task<(TouristPlacesDto? location, string message)> AddPlaceAsync(NewTouristPlacesDto newPlace)
    {
        var placeToAdd = _mapper.Map<TouristPlace>(newPlace);

        foreach (var img in newPlace.Images)
        {
            var res = await _mediaService.AddPhotoAsync(img);
            if (!res.Success)
                return (null, "Failed to add images");
            
            placeToAdd.Photos.Add(new Photo{PictureUrl = res.Url});
        }
        _unitOfWork.Repository<TouristPlace>().Add(placeToAdd);

        if (!await _unitOfWork.SaveChanges())
            return (null, "Failed to add new place");

        var placeToReturn = _mapper.Map<TouristPlacesDto>(placeToAdd);
        return (placeToReturn, "Ok");
    }

    public async Task<(TouristPlacesDto? location, string message)> UpdatePlaceAsync(int id,
        UpdateTouristPlacesDto place)
    {
        var dbPlace = await _unitOfWork.Repository<TouristPlace>().GetQueryable()
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (dbPlace == null)
            return (null, "Place not found");

        _mapper.Map(place, dbPlace);

        
        _unitOfWork.Repository<TouristPlace>().Update(dbPlace);

        if (await _unitOfWork.SaveChanges())
            return (_mapper.Map<TouristPlacesDto>(dbPlace), "Ok");

        return (null, "Failed to update place");
    }

    public async Task<(bool succeed, string message)> DeletePlaceByIdAsync(int id)
    {
        var place = await _unitOfWork.Repository<TouristPlace>().GetQueryable()
            .Include(p=>p.Photos)
            .Where(c=>c.Id == id)
            .FirstOrDefaultAsync();

        if (place == null)
            return (false, "Place not found");
        foreach (var p in place.Photos)
        {
            _unitOfWork.Repository<Photo>().Delete(p);
        }
        place.Photos.Clear();
        _unitOfWork.Repository<TouristPlace>().Delete(place);
        if (await _unitOfWork.SaveChanges())
            return (true, "place deleted successfully");
        return (false, "Failed to delete place");
    }

    public async Task<(bool succeed, string message)> AddPhotosAsync(int placeId,ICollection<IFormFile> images)
    {
        
        var dbPlace = await _unitOfWork.Repository<TouristPlace>().GetQueryable()
            .Include(c => c.Photos)
            .Where(p => p.Id == placeId)
            .FirstOrDefaultAsync();
      
        if (dbPlace == null)
            return (false, "Place not found");
        foreach (var img in images)
        {
            var res = await _mediaService.AddPhotoAsync(img);
            if (!res.Success)
                return (false, "Failed to add images");
            
            dbPlace.Photos.Add(new Photo{PictureUrl = res.Url});
        }
        _unitOfWork.Repository<TouristPlace>().Update(dbPlace);

        if (!await _unitOfWork.SaveChanges())
            return (false, "Failed to add images");

        return (true, "images added successfully");
    }

    public async Task<(bool succeed, string message)> DeletePhotoAsync(int placeId,int imageId)
    {
        var dbPlace = await _unitOfWork.Repository<TouristPlace>().GetQueryable()
            .Include(c => c.Photos)
            .Where(p => p.Id == placeId)
            .FirstOrDefaultAsync();
        
        if (dbPlace == null)
            return (false, "Place not found");

        var image = dbPlace.Photos.FirstOrDefault(c => c.Id == imageId);
        
        if (image == null)
            return (false, "photo not found");

        dbPlace.Photos.Remove(image);
        _unitOfWork.Repository<Photo>().Delete(image);
        _unitOfWork.Repository<TouristPlace>().Update(dbPlace);
        
        if (await _unitOfWork.SaveChanges())
            return (true, "Deleted successfully");
        
        return (false, "Failed to delete image");
    }
}