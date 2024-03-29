﻿using System.ComponentModel.DataAnnotations;

namespace ProjectP.Dtos.AccountDtos;

public class UserInfoDto
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Gender { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PictureUrl { get; set; }
    public string CountryPrifix { get; set; }
}