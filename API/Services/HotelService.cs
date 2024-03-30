using System.Diagnostics;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data.Entities;
using ProjectP.Dtos;
using ProjectP.Dtos.HotelDtos;
using ProjectP.Dtos.OfferDtos;
using ProjectP.Interfaces;

namespace ProjectP.Services;

public class HotelService : IHotelService
{
    
        double radiusInKm = 158;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaService _mediaService;
    private readonly IMapper _mapper;

    public HotelService(IUnitOfWork unitOfWork, IMediaService mediaService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mediaService = mediaService;
        _mapper = mapper;
    }


    public async Task<Hotel?> GetHotelById(int id)
    {
        var hotel = await _unitOfWork.Repository<Hotel>().GetQueryable()
            .Include(p => p.Photos)
            .Include(l => l.Location)
            .Include(o => o.Offer)
            .Include(c => c.HotelRoomTypes).ThenInclude(t => t.RoomType)
            .FirstOrDefaultAsync(x => x.Id == id);
        return hotel;
    }

    public async Task<List<HotelDto>> GetAllHotelsWithOffer()
    {
        var query = _unitOfWork.Repository<Hotel>().GetQueryable();

        query = query.Include(p => p.Photos)
            .Include(l => l.Location)
            .Include(o => o.Offer)
            .Include(c => c.HotelRoomTypes).ThenInclude(t => t.RoomType);
        query = query.Where(c => c.Offer != null);
        var hotel = await query.ToListAsync();

        return _mapper.Map<List<HotelDto>>(hotel);
    }

    public async Task<List<Hotel>> GetAllHotels(HotelFilterParameters? filterParams = null)
    {
        var query = _unitOfWork.Repository<Hotel>().GetQueryable();

        query = query.Include(p => p.Photos)
            .Include(l => l.Location)
            .Include(o => o.Offer)
            .Include(c => c.HotelRoomTypes).ThenInclude(t => t.RoomType);

        if (filterParams != null)
        {
            query = ApplyFilter(filterParams, query);
        }

        var hotels = await query.ToListAsync();

        if (filterParams != null)
        {
            if (filterParams.Latitude != null && filterParams.Longitude != null)
            {
                hotels = hotels.Where(place => CalculateDistance(filterParams.Latitude.Value,
                    filterParams.Longitude.Value,
                    place.Location.Latitude, place.Location.Longitude) <= radiusInKm).ToList();
            }
        }

        return hotels;
    }

    private IQueryable<Hotel> ApplyFilter(HotelFilterParameters filterParams, IQueryable<Hotel> query)
    {
        if (filterParams.MinStars != null)
            query = query.Where(h => h.Stars >= filterParams.MinStars);

        if (filterParams.MaxStars != null)
            query = query.Where(h => h.Stars <= filterParams.MaxStars);

        if (filterParams.MinPrice == null)
            filterParams.MinPrice = 0;

        if (filterParams.MaxPrice == null)
            filterParams.MaxPrice = 1e9;

        query = query.Where(c => !(c.MinPrice > filterParams.MaxPrice || c.MaxPrice < filterParams.MinPrice));

        if (filterParams.RoomType != null && filterParams.RoomType.Trim().Length > 0)
            query = query.Where(h =>
                h.HotelRoomTypes.Any(c => c.RoomType.EnglishName.ToLower() == filterParams.RoomType.ToLower()));

        return query;
    }

    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // Radius of the earth in kilometers
        var dLat = Deg2Rad(lat2 - lat1);
        var dLon = Deg2Rad(lon2 - lon1);
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c; // Distance in kilometers
        return d;
    }

    static double Deg2Rad(double deg)
    {
        return deg * (Math.PI / 180);
    }

    private bool CheckIfIntersect(double cMinPrice, double cMaxPrice, double filterParamsMinPrice,
        double filterParamsMaxPrice)
    {
        if (cMaxPrice < filterParamsMinPrice || cMinPrice > filterParamsMaxPrice)
            return false;
        Console.WriteLine(cMinPrice + " " + cMaxPrice + " " + filterParamsMinPrice + " " + filterParamsMaxPrice);
        return true;
    }

    public async Task<(Hotel? Hotel, string Message)> AddHotel(NewHotelDto hotelDto)
    {
        var hotelToAdd = _mapper.Map<Hotel>(hotelDto);

        foreach (var imageFile in hotelDto.Photos)
        {
            var imageResult = await _mediaService.AddPhotoAsync(imageFile);

            if (!imageResult.Success)
                return (null, imageResult.Message);

            hotelToAdd.Photos.Add(new Photo { PictureUrl = imageResult.Url });
        }

        var roomTypes = await _unitOfWork.Repository<RoomType>().ListAllAsync();
        var roomTypeNames = roomTypes.Select(c => c.EnglishName.ToLower()).ToList();

        foreach (var room in hotelDto.RoomTypes)
        {
            var lower = room.ToLower();

            if (roomTypeNames.All(r => r != lower))
                return (null, $"{room} not found as a room type");
            var type = roomTypes.First(c => c.EnglishName.ToLower() == lower);

            hotelToAdd.HotelRoomTypes.Add(new HotelRoomType
            {
                RoomType = type,
                Hotel = hotelToAdd
            });
        }

        _unitOfWork.Repository<Hotel>().Add(hotelToAdd);
        if (await _unitOfWork.SaveChanges())
            return (hotelToAdd, "Done");
        return (null, "Failed to add hotel");
    }

    public async Task<(bool Succeed, string Message)> DeleteHotel(int hotelId)
    {
        var hotel = await _unitOfWork.Repository<Hotel>().GetQueryable().Include(p => p.Photos)
            .Where(c => c.Id == hotelId).FirstOrDefaultAsync();

        if (hotel == null)
            return (false, "Hotel not found");

        var offers = await _unitOfWork.Repository<Offer>().GetQueryable()
            .Where(c => c.HotelId == hotelId)
            .ToListAsync();

        foreach (var offer in offers)
        {
            _unitOfWork.Repository<Offer>().Delete(offer);
        }

        foreach (var p in hotel.Photos)
        {
            _unitOfWork.Repository<Photo>().Delete(p);
        }

        hotel.Photos.Clear();
        _unitOfWork.Repository<Hotel>().Delete(hotel);

        if (!await _unitOfWork.SaveChanges())
            return (false, "Failed to delete hotel");

        foreach (var photo in hotel.Photos)
        {
            await _mediaService.DeletePhotoAsync(photo.PictureUrl);
        }

        return (true, "Deleted Successfully");
    }

    public async Task<(Hotel? Hotel, string Message)> UpdateHotel(int hotelId, UpdateHotelDto hotelDto)
    {
        var hotel = await _unitOfWork.Repository<Hotel>().GetQueryable()
            .Include(p => p.Photos)
            .Include(l => l.Location)
            .Include(o => o.Offer)
            .Include(c => c.HotelRoomTypes).ThenInclude(t => t.RoomType)
            .Where(h => h.Id == hotelId)
            .FirstOrDefaultAsync();

        if (hotel == null)
            return (null, "Hotel not found");

        var roomTypes = await _unitOfWork.Repository<RoomType>().ListAllAsync();
        var roomTypeNames = roomTypes.Select(c => c.EnglishName.ToLower()).ToList();
        var hotelRoomTypes = hotel.HotelRoomTypes.Select(c => c.RoomType).ToList();

        if (hotelDto.RoomTypes != null)
        {
            hotelDto.RoomTypes = hotelDto.RoomTypes.Select(r => r.ToLower()).ToList();
            foreach (var type in hotelDto.RoomTypes.Where(type => roomTypeNames.All(n => n != type)))
            {
                return (null, $"{type} is a wrong room type");
            }

            UpdateRooms(hotelDto.RoomTypes, hotelRoomTypes, roomTypes, hotel);
        }

        _mapper.Map(hotelDto, hotel);
        _unitOfWork.Repository<Hotel>().Update(hotel);

        if (await _unitOfWork.SaveChanges())
            return (hotel, "Done");
        return (null, "Failed to update hotel");
    }

    private static void UpdateRooms(List<string> roomTypesDto, List<RoomType> hotelRoomTypes,
        IReadOnlyList<RoomType> roomTypes, Hotel hotel)
    {
        var roomToDelete = (from roomType in hotelRoomTypes
            let exist = roomTypesDto.Any(c => c == roomType.EnglishName.ToLower())
            where !exist
            select roomType).ToList();

        var roomToAdd = roomTypesDto.Select(type => roomTypes.First(c => c.EnglishName.ToLower() == type))
            .Where(tmp => hotelRoomTypes.FirstOrDefault(c => c.Id == tmp.Id) == null).ToList();

        var toDelete = hotel.HotelRoomTypes.Where(c => roomToDelete.Select(id => id.Id).Contains(c.RoomTypeId))
            .ToList();

        foreach (var hotelRoomType in toDelete)
        {
            hotel.HotelRoomTypes.Remove(hotelRoomType);
        }

        foreach (var hotelRoomType in roomToAdd)
        {
            hotel.HotelRoomTypes.Add(new HotelRoomType
            {
                RoomType = hotelRoomType
            });
        }
    }

    public async Task<(bool Succeed, string Message)> AddPhotos(int hotelId, ICollection<IFormFile> imageFiles)
    {
        var hotel = await _unitOfWork.Repository<Hotel>().GetByIdAsync(hotelId);

        if (hotel == null)
            return (false, "Hotel not found");
        foreach (var imageFile in imageFiles)
        {
            var imageResult = await _mediaService.AddPhotoAsync(imageFile);

            if (!imageResult.Success)
                return (false, imageResult.Message);

            hotel.Photos.Add(new Photo { PictureUrl = imageResult.Url });
        }

        _unitOfWork.Repository<Hotel>().Update(hotel);
        if (await _unitOfWork.SaveChanges())
            return (true, "Done");
        return (false, "Failed to add Photos");
    }

    public async Task<(bool Succeed, string Message)> DeletePhoto(int hotelId, int photoId)
    {
        var hotel = await _unitOfWork.Repository<Hotel>().GetQueryable()
            .Include(p => p.Photos)
            .Where(c => c.Id == hotelId).FirstOrDefaultAsync();

        if (hotel == null)
            return (false, "Hotel not found");
        var photo = hotel.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo == null)
            return (false, "Photo not found");

        hotel.Photos.Remove(photo);
        _unitOfWork.Repository<Hotel>().Update(hotel);

        if (!await _unitOfWork.SaveChanges())
            return (false, "Failed to delete photo");

        return (true, "Deleted successfully");
    }
}