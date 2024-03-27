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

    public TouristPlacesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<TouristPlacesDto>> GetAllPlacesAsync()
    {
        var dbPlaces = await _unitOfWork.Repository<TouristPlace>().GetQueryable()
            .Include(c => c.Location)
            .ToListAsync();

        return _mapper.Map<List<TouristPlacesDto>>(dbPlaces);
    }

    public async Task<TouristPlacesDto?> GetPlaceByIdAsync(int id)
    {
        var dbPlace = await _unitOfWork.Repository<TouristPlace>().GetQueryable()
            .Include(c => c.Location)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (dbPlace == null)
            return null;

        return _mapper.Map<TouristPlacesDto>(dbPlace);
    }

    public async Task<(TouristPlacesDto? location, string message)> AddPlaceAsync(NewTouristPlacesDto newPlace)
    {
        var placeToAdd = _mapper.Map<TouristPlace>(newPlace);
        placeToAdd.Location = new Location
        {
            StreetName = "",
            City = newPlace.City,
            Longitude = newPlace.Longitude,
            Latitude = newPlace.Latitude
        };

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
            .Include(c => c.Location)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (dbPlace == null)
            return (null, "Place not found");

        _mapper.Map(place, dbPlace);

        if (place.City != null)
            dbPlace.Location.City = place.City;

        if (place.Latitude != null)
            dbPlace.Location.Latitude = place.Latitude.Value;

        if (place.Longitude != null)
            dbPlace.Location.Longitude = place.Longitude.Value;

        _unitOfWork.Repository<TouristPlace>().Update(dbPlace);

        if (await _unitOfWork.SaveChanges())
            return (_mapper.Map<TouristPlacesDto>(dbPlace), "Ok");

        return (null, "Failed to update place");
    }

    public async Task<(bool succeed, string message)> DeletePlaceByIdAsync(int id)
    {
        var place = await _unitOfWork.Repository<TouristPlace>().GetByIdAsync(id);

        if (place == null)
            return (false, "Place not found");

        _unitOfWork.Repository<TouristPlace>().Delete(place);
        if (await _unitOfWork.SaveChanges())
            return (true, "place deleted successfully");
        return (false, "Failed to delete place");
    }
}