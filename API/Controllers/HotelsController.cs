using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectP.Data.Entities;
using ProjectP.Dtos.HotelDtos;
using ProjectP.Errors;
using ProjectP.Interfaces;

namespace ProjectP.Controllers;

public class HotelsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IHotelService _hotelService;

    public HotelsController(IMapper mapper, IHotelService hotelService)
    {
        _mapper = mapper;
        _hotelService = hotelService;
    }

    [HttpGet]
    public async Task<ActionResult<List<HotelDto>>> GetAllHotels()
    {
        var hotels = await _hotelService.GetAllHotels();
        return Ok(new ApiOkResponse<List<HotelDto>>(_mapper.Map<List<HotelDto>>(hotels)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotel = await _hotelService.GetHotelById(id);
        if (hotel != null)
        return Ok(new ApiOkResponse<HotelDto>(_mapper.Map<HotelDto>(hotel)));
        
        return Ok(new ApiResponse(404, "Hotel not found"));
    }

    [HttpPost]
    public async Task<ActionResult<HotelDto>> AddHotel([FromForm] NewHotelDto hotelDto)
    {
        var result = await _hotelService.AddHotel(hotelDto);

        if (result.Hotel == null)
            return Ok(new ApiResponse(400, result.Message));
        return Ok(new ApiOkResponse<HotelDto>(_mapper.Map<HotelDto>(result.Hotel)));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<HotelDto>> UpdateHotel(int id, UpdateHotelDto hotelDto)
    {
        var result = await _hotelService.UpdateHotel(id, hotelDto);

        if (result.Hotel == null)
            return Ok(new ApiResponse(400, result.Message));
        return Ok(new ApiOkResponse<HotelDto>(_mapper.Map<HotelDto>(result.Hotel)));
    }

    [HttpPost("{id:int}/add-photo")]
    public async Task<ActionResult> AddPhoto(int id, [FromForm] ICollection<IFormFile> imageFiles)
    {
        var result = await _hotelService.AddPhotos(id, imageFiles);

        if (result.Succeed) return Ok(new ApiResponse(200, result.Message));
        return Ok(new ApiResponse(400, result.Message));
    }

    [HttpDelete("{hotelId:int}/delete-photo")]
    public async Task<ActionResult> DeletePhoto(int hotelId, int photoId)
    {
        var result = await _hotelService.DeletePhoto(hotelId, photoId);

        if (result.Succeed) return Ok(new ApiResponse(200, result.Message));
        return Ok(new ApiResponse(400, result.Message));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteHotel(int id)
    {
        var result = await _hotelService.DeleteHotel(id);

        if (result.Succeed)
            return Ok(new ApiResponse(200, result.Message));
        return Ok(new ApiResponse(400, result.Message));
    }
}