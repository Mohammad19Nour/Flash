using AsparagusN.Enums;
using ProjectP.Interfaces;

namespace ProjectP.Services;

public class MediaService : IMediaService
{
     public MediaService()
    {
    }

    public async Task<(bool, string, string)> AddPhotoAsync(IFormFile file)
    {
        try
        {
            var fileExtension = Path.GetExtension(file.FileName);

            if (!(Enum.TryParse(fileExtension.TrimStart('.'), ignoreCase: true,
                      out ImageExtension parsedExtension)
                       && Enum.IsDefined(typeof(ImageExtension), parsedExtension)))
                return (false, "", "Image Extension not supported");

            var fileName = _getFileName(file,"images");

            var uploadPath = Path.Combine("wwwroot/images/", fileName);

            var stream = new FileStream(uploadPath, FileMode.Create);

            await file.CopyToAsync(stream);
            await stream.DisposeAsync();

            var imageLink = "images/" + fileName;
            return (true, imageLink, "done");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (false, "", "Failed tp upload photo");
        }
    }

    public async Task<(bool Success, string Url, string Message)> AddVideoAsync(IFormFile file)
    {
        try
        {
            var fileExtension = Path.GetExtension(file.FileName);

            if (!(Enum.TryParse(fileExtension.TrimStart('.'), ignoreCase: true,
                      out VideoExtension parsedExtension)
                  && Enum.IsDefined(typeof(VideoExtension), parsedExtension)))
                return (false, "", "Video Extension not supported");

            var fileName = _getFileName(file,"videos");

            var uploadPath = Path.Combine("wwwroot/videos/", fileName);

            var stream = new FileStream(uploadPath, FileMode.Create);

            await file.CopyToAsync(stream);
            await stream.DisposeAsync();

            var imageLink = "videos/" + fileName;
            return (true, imageLink, "done");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (false, "", "Failed tp upload photo");
        }
    }


    public async Task<bool> DeletePhotoAsync(string name)
    {
        // var deleteParams = new DeletionParams(publicId);
        //var result = await _cloudinary.DestroyAsync(deleteParams);

        // name = name[8..];
        if (File.Exists("wwwroot" + name)) //check file exist or not  
        {
            File.Delete("wwwroot" + name);
            return true;
        }

        return false;
    }
    private string _getFileName(IFormFile file , string directoryName)
    {
        var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", directoryName);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var fileName = file.FileName.Where(c => c != ' ').Aggregate("", (current, c) => current + c);

        return DateTime.Now.ToString("yyyyMMddHHmmss_") + Path.GetFileName(fileName);
    }
}