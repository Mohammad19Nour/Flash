using ProjectP.Data.Entities;
using ProjectP.Interfaces;

namespace ProjectP.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomTypeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<RoomType>> GetAllRoomTypesAsync()
    {
        return  (await _unitOfWork.Repository<RoomType>().ListAllAsync()).ToList();
    }

    public async Task<bool> CheckIfRoomExistByEnglishNameAsync(string englishName)
    {
        var rooms = await GetAllRoomTypesAsync();
        var names = rooms.Select(c => c.EnglishName.ToLower()).ToList();

        return names.Contains(englishName.ToLower());
    }
}