using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectP.Data.Entities.Identity;
using ProjectP.Dtos.AccountDtos;
using ProjectP.Errors;
using ProjectP.Extensions;
using ProjectP.Interfaces;

namespace ProjectP.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public UsersController(IUnitOfWork unitOfWork,IMapper mapper,
        IMediaService mediaService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediaService = mediaService;
    }
    
    [HttpGet]
    public async Task<ActionResult<UserInfoDto>> GetUserInfo()
    {
        try
        {
            var user = await GetUser();
            if (user == null) return Ok(new ApiResponse(401, "Unauthorized"));

            return Ok(new ApiOkResponse<UserInfoDto>(_mapper.Map<UserInfoDto>(user)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    [HttpPut("info")]
    public async Task<ActionResult> UpdateUser([FromForm] UpdateUserInfoDto dto)
    {
        try
        {
            var user = await GetUser();
            if (user == null) return Ok(new ApiResponse(401, "Unauthorized"));

            _mapper.Map(dto, user);
            if (dto.Image != null)
            {
                var photoResult = await _mediaService.AddPhotoAsync(dto.Image);
                if (!photoResult.Success) return Ok(new ApiResponse(400, photoResult.Message));
                user.PictureUrl = photoResult.Url;
            }

           
            _unitOfWork.Repository<AppUser>().Update(user);

            return Ok(await _unitOfWork.SaveChanges()
                ? new ApiOkResponse<UserInfoDto>(_mapper.Map<UserInfoDto>(user))
                : new ApiResponse(400, messageEN: "Failed to update"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private async Task<AppUser?> GetUser()

    {
        var email = User.GetEmail();
        var user = await _unitOfWork.Repository<AppUser>().GetQueryable().Where(c=>c.Email.ToLower() == email).FirstOrDefaultAsync();
        return user;
    }
}