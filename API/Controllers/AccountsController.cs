using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectP.Dtos.AccountDtos;
using ProjectP.Errors;
using ProjectP.Interfaces;

namespace ProjectP.Controllers;

public class AccountsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IAccountService _accountService;

    public AccountsController(IMapper mapper, IAccountService accountService)
    {
        _mapper = mapper;
        _accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AccountDto>> Login(LoginDto loginDto)
    {
        var result = await _accountService.Login(loginDto);
        if (result.user == null) return Ok(new ApiResponse(400, result.Message));
        return Ok(new ApiOkResponse<AccountDto>(result.user));
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromForm] RegisterDto registerDto)
    {
        var result = await _accountService.Register(registerDto);
        if (result.Success) return Ok(new ApiResponse(200, result.Message));
        else return Ok(new ApiResponse(400, result.Message));
    }

    [HttpPost("forget-password")]
    public async Task<ActionResult> ForgetPassword(ForgetPasswordDto dto)
    {
        var result = await _accountService.ForgetPassword(dto);
        if (result.Success) return Ok(new ApiResponse(200, result.Message));
        else return Ok(new ApiResponse(400, result.Message));
    }


    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult> ResetPassword(ResetDto restDto)
    {
        var result = await _accountService.ResetPassword(restDto);
        if (result.Success) return Ok(new ApiResponse(200, result.Message));
        else return Ok(new ApiResponse(400, result.Message));
    }
}