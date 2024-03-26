using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectP.Dtos.OfferDtos;
using ProjectP.Errors;
using ProjectP.Interfaces;

namespace ProjectP.Controllers;

public class OffersController : BaseApiController
{
    private readonly IOfferService _offerService;
    private readonly IMapper _mapper;

    public OffersController(IOfferService offerService, IMapper mapper)
    {
        _offerService = offerService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<OfferDto>>> GetAllOffer()
    {
        var offers = await _offerService.GetAllOffersAsync();

        return Ok(new ApiOkResponse<List<OfferDto>>(offers));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OfferDto>> GetOfferById(int id)
    {
        var result = await _offerService.GetOfferByIdAsync(id);

        if (result.offer == null)
            return Ok(new ApiResponse(400, result.message));

        return Ok(new ApiOkResponse<OfferDto>(_mapper.Map<OfferDto>(result.offer)));
    }

    [HttpPost]
    public async Task<ActionResult<OfferDto>> AddOffer(NewOfferDto offerDto)
    {
        var result = await _offerService.AddOffer(offerDto);
        if (result.offer == null)
            return Ok(new ApiResponse(400, result.message));

        return Ok(new ApiOkResponse<OfferDto>(_mapper.Map<OfferDto>(result.offer)));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<OfferDto>> UpdateOffer(int id, UpdateOfferDto offerDto)
    {
        var result = await _offerService.UpdateOffer(id, offerDto);
        if (result.offer == null)
            return Ok(new ApiResponse(400, result.message));

        return Ok(new ApiOkResponse<OfferDto>(_mapper.Map<OfferDto>(result.offer)));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteOffer(int id)
    {
        var result = await _offerService.DeleteOffer(id);
        if (!result.succeed)
            return Ok(new ApiResponse(400, result.message));

        return Ok(new ApiResponse(200, result.message));
    }
}