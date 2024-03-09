namespace ProjectP.Interfaces;

public interface IMediaService
{
    Task< (bool Success , string Url,string Message)> AddPhotoAsync(IFormFile file);
    Task< (bool Success , string Url,string Message)> AddVideoAsync(IFormFile file);
    Task<bool> DeletePhotoAsync(string publicId);
}