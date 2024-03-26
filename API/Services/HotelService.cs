using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data.Entities;
using ProjectP.Dtos.HotelDtos;
using ProjectP.Dtos.OfferDtos;
using ProjectP.Interfaces;

namespace ProjectP.Services;

public class HotelService : IHotelService
{
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
            .Include(o => o.Offer)
            .FirstOrDefaultAsync(x => x.Id == id);
        return hotel;
    }

    public async Task<ICollection<Hotel>> GetAllHotels()
    {
        var hotels = await _unitOfWork.Repository<Hotel>().GetQueryable()
            .Include(p => p.Photos)
            .Include(l => l.Location)
            .Include(o => o.Offer)
            .ToListAsync();
        return hotels;
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

        _unitOfWork.Repository<Hotel>().Add(hotelToAdd);
        if (await _unitOfWork.SaveChanges())
            return (hotelToAdd, "Done");
        return (null, "Failed to add hotel");
    }

    public async Task<(bool Succeed, string Message)> DeleteHotel(int hotelId)
    {
        var hotel = await _unitOfWork.Repository<Hotel>().GetByIdAsync(hotelId);

        if (hotel == null)
            return (false, "Hotel not found");

        var offers = await _unitOfWork.Repository<Offer>().GetQueryable()
            .Where(c => c.HotelId == hotelId)
            .ToListAsync();

        foreach (var offer in offers)
        {
            _unitOfWork.Repository<Offer>().Delete(offer);
        }

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
            .Where(h => h.Id == hotelId)
            .FirstOrDefaultAsync();

        if (hotel == null)
            return (null, "Hotel not found");

        _mapper.Map(hotelDto, hotel);
        _unitOfWork.Repository<Hotel>().Update(hotel);
        if (await _unitOfWork.SaveChanges())
            return (hotel, "Done");
        return (null, "Failed to update hotel");
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