﻿using ProjectP.Data.Entities;
using ProjectP.Dtos.HotelDtos;

namespace ProjectP.Interfaces;

public interface IHotelService
{
    public Task<Hotel?> GetHotelById(int id);
    public Task<ICollection<Hotel>> GetAllHotels();
    public Task<(Hotel? Hotel, string Message)> AddHotel(NewHotelDto hotelDto);
    public Task<(bool Succeed, string Message)> DeleteHotel(int hotelId);
    public Task<(Hotel? Hotel, string Message)> UpdateHotel(int hotelId, UpdateHotelDto hotelDto);
    public Task<(bool Succeed, string Message)> AddPhotos(int hotelId,ICollection<IFormFile> imageFiles);
    public Task<(bool Succeed, string Message)> DeletePhoto(int hotelId,int photoId);
}