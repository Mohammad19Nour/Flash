using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data.Entities;
using ProjectP.Data.Entities.Identity;
using ProjectP.Dtos.HotelDtos;
using ProjectP.Interfaces;

namespace ProjectP.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOfferService _offerService;
    private readonly IHotelService _hotelService;

    public FavoriteService(IUnitOfWork unitOfWork, IMapper mapper, IOfferService offerService,
        IHotelService hotelService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _offerService = offerService;
        _hotelService = hotelService;
    }

    public async Task<(bool succeed, string message)> ToggleHotelInFavorite(int hotelId, string userEmail)
    {
        var user = await _getUserByEmailAsync(userEmail);
        if (user == null) return (false, "User not found");

        var hotel = await _hotelService.GetHotelById(hotelId);
        if (hotel == null) return (false, "Hotel not found");

        var uh = await _getHotelInFavoriteListForUserAsync(hotel.Id, user.Id);

        if (uh != null) // Exist.. so we will remove it from favorite
            return await RemoveFromFavorite(hotelId, user.Id);
        var userHotel = new UserHotel
        {
            User = user,
            Hotel = hotel
        };

        _unitOfWork.Repository<UserHotel>().Add(userHotel);

        if (await _unitOfWork.SaveChanges())
            return (true, "Hotel added to the favorite list");
        return (false, "Failed to add the favorite list");
    }

    private async Task<(bool succeed, string message)> RemoveFromFavorite(int hotelId, int userId)
    {
        var uh = await _getHotelInFavoriteListForUserAsync(hotelId, userId);

        if (uh != null)
            _unitOfWork.Repository<UserHotel>().Delete(uh);

        if (await _unitOfWork.SaveChanges())
            return (true, "Hotel removed from favorite");

        return (false, "Failed to remove hotel from favorite");
    }

    public async Task<List<HotelDto>> GetFavoriteHotels(string userEmail)
    {
        var user = await _getUserByEmailAsync(userEmail);
        if (user == null)
            return new List<HotelDto>();
        
        var query = _unitOfWork.Repository<UserHotel>().GetQueryable();
        query = query.Include(c => c.Hotel).ThenInclude(h => h.Photos);
        query = query.Include(c => c.Hotel).ThenInclude(h => h.Location);
        query = query.Include(c => c.Hotel).ThenInclude(h => h.Offer);
        query = query.Where(c => c.UserId == user.Id);

        var hotels = (await query.ToListAsync()).Select(u => u.Hotel).ToList();

        var result = _mapper.Map<List<HotelDto>>(hotels);
        foreach (var hotel in result)
        {
            hotel.IsFavorite = true;
        }

        return result;
    }

    private async Task<AppUser?> _getUserByEmailAsync(string email)
    {
        email = email.ToLower();
        var user = await _unitOfWork.Repository<AppUser>().GetQueryable()
            .Where(c => c.Email.ToLower() == email).FirstOrDefaultAsync();
        return user;
    }

    private async Task<UserHotel?> _getHotelInFavoriteListForUserAsync(int hotelId, int userId)
    {
        var userHotel = await _unitOfWork.Repository<UserHotel>().GetQueryable()
            .Where(uh => uh.HotelId == hotelId && uh.UserId == userId).FirstOrDefaultAsync();

        return userHotel;
    }
}