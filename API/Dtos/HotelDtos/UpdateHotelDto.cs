using System.ComponentModel.DataAnnotations;
using ProjectP.Dtos.LocationDtoss;

namespace ProjectP.Dtos.HotelDtos;

public class UpdateHotelDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int? Stars { get; set; }
    public int? Reviews { get; set; }

    public string? Description { get; set; }
    [Range(0, double.MaxValue)] public double? MinPrice { get; set; }
    [Range(0, double.MaxValue)] public double? MaxPrice { get; set; }
    public UpdateLocationDto? Location { get; set; }
}