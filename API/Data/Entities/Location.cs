﻿namespace ProjectP.Data.Entities;

public class Location
{
    public int Id { get; set; }
    public string City { get; set; }
    public string StreetName { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}