using Microsoft.AspNetCore.Mvc;
using ProjectP.Dtos.TouristPlacesDtos;
using ProjectP.Errors;
using ProjectP.Interfaces;

namespace ProjectP.Controllers;

public class TouristsPlacesController : BaseApiController
{
    private readonly ITouristPlacesService _touristPlacesService;

    public TouristsPlacesController(ITouristPlacesService touristPlacesService)
    {
        _touristPlacesService = touristPlacesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TouristPlacesDto>>> GetAllPlaces()
    {
        var places = await _touristPlacesService.GetAllPlacesAsync();

        return Ok(new ApiOkResponse<List<TouristPlacesDto>>(places));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TouristPlacesDto>> GetPlace(int id)
    {
        var place = await _touristPlacesService.GetPlaceByIdAsync(id);
        if (place == null)
            return Ok(new ApiResponse(400, "Place not found"));

        return Ok(new ApiOkResponse<TouristPlacesDto>(place));
    }

    [HttpPost]
    public async Task<ActionResult<TouristPlacesDto>> AddPlace([FromForm]NewTouristPlacesDto newPlace)
    {
        var result = await _touristPlacesService.AddPlaceAsync(newPlace);

        if (result.location != null)
            return Ok(new ApiOkResponse<TouristPlacesDto>(result.location));

        return Ok(new ApiResponse(400, result.message));
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TouristPlacesDto>> UpdatePlace(UpdateTouristPlacesDto updatedPlace, int id)
    {
        var result = await _touristPlacesService.UpdatePlaceAsync(id,updatedPlace);

        if (result.location != null)
            return Ok(new ApiOkResponse<TouristPlacesDto>(result.location));

        return Ok(new ApiResponse(400, result.message));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePlace(int id)
    {
        var result = await _touristPlacesService.DeletePlaceByIdAsync(id);

        if (result.succeed)
            return Ok(new ApiResponse(200, result.message));
        
        return Ok(new ApiResponse(400, result.message));

    }
    [HttpPost("{id:int}/photo")]
    public async Task<ActionResult> AddPhoto(int id, [FromForm] ICollection<IFormFile> imageFiles)
    {
        var result = await _touristPlacesService.AddPhotosAsync(id, imageFiles);

        if (result.succeed) return Ok(new ApiResponse(200, result.message));
        return Ok(new ApiResponse(400, result.message));
    }

    [HttpDelete("{placeId:int}/photo")]
    public async Task<ActionResult> DeletePhoto(int placeId, int photoId)
    {
        var result = await _touristPlacesService.DeletePhotoAsync(placeId, photoId);

        if (result.succeed) return Ok(new ApiResponse(200, result.message));
        return Ok(new ApiResponse(400, result.message));
    }
}