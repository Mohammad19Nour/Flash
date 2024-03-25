using System.Security.Claims;
using System.Web;
using AsparagusN.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using ProjectP.Data.Entities.Identity;
using ProjectP.Dtos.AccountDtos;
using ProjectP.Entities;
using ProjectP.Errors;
using ProjectP.Extensions;
using ProjectP.Interfaces;

namespace ProjectP.Services;

public class AccountService : IAccountService
{
    private readonly IUrlHelper _urlHelper;
    private readonly IConfiguration _config;
    private readonly IMediaService _mediaService;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailService _emailService;

    public AccountService(IConfiguration config, IMediaService mediaService, ITokenService tokenService,
        UserManager<AppUser> userManager,
        IMapper mapper,
        SignInManager<AppUser> signInManager, IEmailService emailService)
    {
        _config = config;
        _mediaService = mediaService;
        _tokenService = tokenService;
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    public async Task<(AccountDto? user, string Message)> Login(LoginDto loginDto)
    {
        loginDto.Email = loginDto.Email.ToLower();
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null)
            return (null, "Invalid Email");
        var isUserRole = await _userManager.IsInRoleAsync(user, Roles.User.GetDisplayName().ToLower());
        if (!isUserRole) return (null, "Can't access to this resource");

        var res = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!res.Succeeded)
            return (null, "Invalid password");

        /*   if (!user.EmailConfirmed)
        {
            var response = await GenerateTokenAndSendEmailForUser(user);

            if (!response)
                return Ok(new ApiResponse(400, messageEN: "Failed to send email.", messageAR: "فشل ارسال الايميل"));

            return Ok(new ApiResponse(200, messageEN: "You have to confirm your account first," +
                                                      "The confirmation link will be resent to your email," +
                                                      " please check it and confirm your account.",
                messageAR:
                "يجب عليك تاكيد الحساب اولا, سيتم إعادة إرسال رابط التأكيد إليك... الرجاء التأكد من صندوق الوارد لديك من اجل تاكيد حسابك"));
        }*/

        var userDto = _mapper.Map<AppUser, AccountDto>(user);
        userDto.Token = _tokenService.CreateToken(user);
        return (userDto, "Done");
    }

    public async Task<(bool Success, string Message)> Register(RegisterDto registerDto)
    {
        try
        {
            registerDto.Email = registerDto.Email.ToLower();

            var user = await _userManager.Users.FirstOrDefaultAsync(c => c.Email.ToLower() == registerDto.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                    return (false, "This email is already used");
                var response = await GenerateTokenAndSendEmailForUser(user);

                if (!response)
                    return (false, "Failed to send email.");

                return (false, "You have already registered with this Email," +
                               "The confirmation link will be resent to your email," +
                               " please check it and confirm your account.");
            }

            if (registerDto.Password != registerDto.ConfirmedPassword)
                return (false, "passwords aren't identical");
           
            user = new AppUser();

            if (registerDto.Image != null)
            {
                var photoRes = await _mediaService.AddPhotoAsync(registerDto.Image);

                if (!photoRes.Success)
                    return (false, photoRes.Message);

            
                user.PictureUrl = photoRes.Url;

            }
            _mapper.Map(registerDto, user);
            user.UserName = registerDto.Email;
            var res = await _userManager.CreateAsync(user, registerDto.Password);

            if (res.Succeeded == false)
                return (false, string.Join(" ", res.Errors.Select(v => v.Description).ToList()));

            IdentityResult roleResult;
            roleResult = await _userManager.AddToRoleAsync(user, "User");


            if (!roleResult.Succeeded) return (false, "Failed to add roles");

            var respons = await GenerateTokenAndSendEmailForUser(user);

            if (!respons)
                return (false, "Failed to send email.");

            return (true, "The confirmation link was send to your email successfully, " +
                          "please check your email and confirm your account.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task<(bool Success, string Message)> UpdatePassword(UpdatePasswordDto dto, string email)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user is null)
                return (false, "Unauthorized");

            var res =
                await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (!res.Succeeded)
                return (false, "Failed to update password");

            return (true, "updated successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<(bool Success, string Message)> ConfirmEmail(string? userId, string? token)
    {
        if (userId is null || token is null)
        {
            return (false, "token or userId missing");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return (false, "invalid token user id");
        }

        var res = await _userManager.ConfirmEmailAsync(user, token);

        string msg = "confirmation failed";
        foreach (var r in res.Errors)
        {
          Console.WriteLine(r.Description);
          msg = r.Description;
        }

        if (!res.Succeeded) return (false, msg);

        return (true, "Your Email is Confirmed try to login in now");
    }

    public async Task<(bool Success, string Message)> ForgetPassword(ForgetPasswordDto dto)
    {
        try
        {
            var email = dto.Email?.ToLower();
            if (email is null) return (false, "email must be provided");
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return (false, "user was not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var code = ConfirmEmailService.GenerateCode(user.Id, token);
            var baseUrl = _config["baseUrl"];
            var confirmationLink = $"{baseUrl}/api/Accounts/ResetPassword?userId={user.Id}&token={token}";


            var text = "<html><body> The code to reset your password is : " + code +
                       "</body></html>";
            var res = await _emailService.SendEmailAsync(user.Email, "Reset Password", text);

            if (!res)
                return (false, "Failed to send email.");

            return (true, "The Code was sent to your email");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<(bool Success, string Message)> ResetPassword(ResetDto restDto)
    {
        var newPassword = restDto.NewPassword;
        var code = restDto.Code;
        if (newPassword == null || code == null)
            return (true, "The password should not be empty");
        var val = ConfirmEmailService.GetUserIdAndToken(code);

        if (val is null)
            return (false, "the code is incorrect");

        var userId = val.Value.userId;
        var token = val.Value.token;

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return (false, "this user is not registered");

        var res = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (res.Succeeded == false) return (false, "Cannot reset password");

        ConfirmEmailService.RemoveUserCodes(userId);
        return (true, "Password was reset successfully");
    }

    private async Task<bool> GenerateTokenAndSendEmailForUser(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var baseUrl = _config["ApiUrl"];
        var codeHtmlVersion = HttpUtility.UrlEncode(token);
        var confirmationLink = $"{baseUrl}api/Accounts/ConfirmEmail?userId={user.Id}&token={codeHtmlVersion}";

        var text = "<html><body>To confirm your email please<a href=" + confirmationLink +
                   "> click here</a></body></html>";
       
        
        Console.WriteLine("generated token: "+token);
        Console.WriteLine("encoded token"+codeHtmlVersion);
        return await _emailService.SendEmailAsync(user.Email, "Confirmation Email", text);
    }
}