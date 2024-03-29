using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data.Entities;
using ProjectP.Dtos.OfferDtos;
using ProjectP.Interfaces;

namespace ProjectP.Services;

public class OfferService : IOfferService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHotelService _hotelService;

    public OfferService(IUnitOfWork unitOfWork, IMapper mapper, IHotelService hotelService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _hotelService = hotelService;
    }

    public async Task<List<OfferDto>> GetAllOffersAsync()
    {
        var offers = await _unitOfWork.Repository<Offer>().GetQueryable()
            .Include(v => v.Hotel)
            .ThenInclude(p=>p.Photos)
            .Include(o=>o.Hotel)
            .ThenInclude(h=>h.Location)
            .Include(o=>o.Hotel)
            .ThenInclude(h=>h.HotelRoomTypes).ThenInclude(c=>c.RoomType)
            .ToListAsync();

        return _mapper.Map<List<OfferDto>>(offers);
    }

    public async Task<(Offer? offer, string message)> GetOfferByIdAsync(int offerId)
    {
        var offer = await _unitOfWork.Repository<Offer>().GetQueryable()
            .Include(v => v.Hotel)
            .ThenInclude(p=>p.Photos)
            .Include(o=>o.Hotel)
            .ThenInclude(h=>h.Location)
            .Include(o=>o.Hotel)
            .ThenInclude(h=>h.HotelRoomTypes).ThenInclude(c=>c.RoomType)
            .Where(h=>h.Id == offerId)
            .FirstOrDefaultAsync();

        if (offer == null)
            return (null, "Offer not found");

        return (offer, "Done");
    }

    public async Task<(Offer? offer, string message)> AddOffer(NewOfferDto offerDto)
    {
        var hotel = await _unitOfWork.Repository<Hotel>().GetByIdAsync(offerDto.HotelId);
        
        if (hotel == null)
            return (null, "Hotel not found");
        
        var offerToAdd = _mapper.Map<Offer>(offerDto);
      //  offerToAdd.Hotel = hotel;
        hotel.Offer = offerToAdd;
        _unitOfWork.Repository<Offer>().Add(offerToAdd);
        _unitOfWork.Repository<Hotel>().Update(hotel);

        if (await _unitOfWork.SaveChanges())
        {
            Console.WriteLine(hotel.OfferId == null);
            return (offerToAdd, "Offer added successfully");
        }

        return (null, "Failed to add offer");
    }

    public async Task<(Offer? offer, string message)> UpdateOffer(int offerId, UpdateOfferDto offerDto)
    {
        var offer = await _unitOfWork.Repository<Offer>().GetByIdAsync(offerId);

        if (offer == null)
            return (null, "Offer not found");

        _mapper.Map(offerDto, offer);
        _unitOfWork.Repository<Offer>().Update(offer);

        if (await _unitOfWork.SaveChanges())
            return (offer, "Updated successfully");

        return (null, "Failed to update offer");
    }

    public async Task<(bool succeed, string message)> DeleteOffer(int offerId)
    {
        var offer = await _unitOfWork.Repository<Offer>().GetByIdAsync(offerId);

        if (offer == null)
            return (false, "Offer not found");

        _unitOfWork.Repository<Offer>().Delete(offer);

        if (await _unitOfWork.SaveChanges())
            return (true, "Offer deleted successfully");

        return (false, "OFailed to delete the offer");
    }
}