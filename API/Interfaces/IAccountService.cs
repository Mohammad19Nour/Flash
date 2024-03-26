using ProjectP.Data.Entities.Identity;
using ProjectP.Dtos.AccountDtos;

namespace ProjectP.Interfaces;

public interface IAccountService
{
    public Task<(AccountDto? user,string Message)> Login(LoginDto loginDto);
    public Task<(bool Success, string Message)> Register(RegisterDto registerDto);
    public Task<(bool Success,string Message)> UpdatePassword(UpdatePasswordDto dto,string userEmail);
    public Task<(bool Success, string Message)> ConfirmEmail(string? userId, string? token);
    public Task<(bool Success, string Message)> ForgetPassword(ForgetPasswordDto dto);
    public Task<(bool Success, string Message)> ResetPassword(ResetDto restDto);
}