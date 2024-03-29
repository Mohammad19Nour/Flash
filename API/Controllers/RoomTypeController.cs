using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectP.Dtos.HotelDtos;
using ProjectP.Errors;
using ProjectP.Interfaces;

namespace ProjectP.Controllers;

public class RoomTypeController : BaseApiController
{
    private readonly IRoomTypeService _roomTypeService;
    private readonly IMapper _mapper;

    public RoomTypeController(IRoomTypeService roomTypeService,IMapper mapper)
    {
        _roomTypeService = roomTypeService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomTypeDto>>> GetAllRoomTypes()
    {
        var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
        var dto = _mapper.Map<List<RoomTypeDto>>(roomTypes);

        return Ok(new ApiOkResponse<List<RoomTypeDto>>(dto));

    }
}