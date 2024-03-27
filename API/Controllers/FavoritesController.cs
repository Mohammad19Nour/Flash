using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectP.Dtos.HotelDtos;
using ProjectP.Errors;
using ProjectP.Extensions;
using ProjectP.Interfaces;

namespace ProjectP.Controllers;
[Authorize]
public class FavoritesController : BaseApiController
{
    private readonly IFavoriteService _favoriteService;

    public FavoritesController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<HotelDto>>> GetFavorites()
    {
        var email = User.GetEmail();
        var hotels = await _favoriteService.GetFavoriteHotels(email);
        return Ok(new ApiOkResponse<List<HotelDto>>(hotels));
    }

    [HttpPost]
    public async Task<ActionResult> ToggleFavorite(int hotelId)
    {
        var email = User.GetEmail();
        var result = await _favoriteService.ToggleHotelInFavorite(hotelId, email);

        if (result.succeed)
            return Ok(new ApiResponse(200, result.message));
        return Ok(new ApiResponse(400, result.message));
    }
}