﻿namespace ProjectP.Data.Entities;

public class Photo
{
    public int Id { get; set; }
    public string PictureUrl { get; set; }
    public Hotel Hotel { get; set; }
    public int HotelId { get; set; }
}