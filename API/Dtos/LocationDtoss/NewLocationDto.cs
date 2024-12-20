﻿using System.ComponentModel.DataAnnotations;

namespace ProjectP.Dtos.LocationDtoss;

public class NewLocationDto
{
    [Required] [MaxLength(100)] public string StreetName { get; set; }
    [Required] [MaxLength(100)] public string City { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}