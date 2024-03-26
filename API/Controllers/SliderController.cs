using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectP.Data.Entities;
using ProjectP.Dtos;
using ProjectP.Dtos.SliderDtos;
using ProjectP.Enums;
using ProjectP.Errors;
using ProjectP.Interfaces;

namespace ProjectP.Controllers;

public class SliderController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaService _mediaService;
    private readonly IMapper _mapper;

    public SliderController(IUnitOfWork unitOfWork, IMediaService mediaService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mediaService = mediaService;
        _mapper = mapper;
    }

    //[Authorize(Roles = nameof(Roles.Admin))]
    [HttpPost]
    public async Task<ActionResult> AddPhoto([FromForm]NewSliderDto dto)
    {
        try
        {
            var result = await _mediaService.AddPhotoAsync(dto.Image);

            if (!result.Success)
                return Ok(new ApiResponse(400, messageEN: result.Message));

            var photo = new Slider()
            {
                PictureUrl = result.Url,
                Text = dto.Text ?? ""
            };
            _unitOfWork.Repository<Slider>().Add(photo);


            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200, messageEN: "Added successfully"));

            return Ok(new ApiResponse(400, messageEN: "Failed to upload photo"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    //[Authorize(Roles = nameof(Roles.Admin))]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePhoto(int id,[FromForm] UpdateSliderDto dto)
    {
        try
        {
            var slider = await _unitOfWork.Repository<Slider>().GetByIdAsync(id);

            if (slider is null) return Ok(new ApiResponse(404, "slider not found"));

            if (dto.Image != null)
            {
                var result = await _mediaService.AddPhotoAsync(dto.Image);

                if (!result.Success)
                    return Ok(new ApiResponse(400, messageEN: result.Message));
                slider.PictureUrl = result.Url;
            }

            if (dto.Text != null)
                slider.Text = dto.Text;

            _unitOfWork.Repository<Slider>().Update(slider);


            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200, messageEN: "Updated successfully"));

            return Ok(new ApiResponse(400, messageEN: "Failed to update slider"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Slider>>> GetSliderPhotos()
    {
        var res = await _unitOfWork.Repository<Slider>().ListAllAsync();
        return Ok(new ApiOkResponse<IReadOnlyList<Slider>>(res));
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult> DeletePhoto(int id)
    {
        try
        {
            var photo = await _unitOfWork.Repository<Slider>().GetByIdAsync(id);

            if (photo is null) return NotFound(new ApiResponse(404, "photo not found"));

            _unitOfWork.Repository<Slider>().Delete(photo);

            if (!await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(400, "Something went wrong"));

            await _mediaService.DeletePhotoAsync(photo.PictureUrl);
            return Ok(new ApiResponse(200, "Deleted successfully"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}