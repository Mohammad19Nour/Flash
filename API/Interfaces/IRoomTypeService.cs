using ProjectP.Data.Entities;

namespace ProjectP.Interfaces;

public interface IRoomTypeService
{ 
    Task<List<RoomType>> GetAllRoomTypesAsync();
    Task<bool> CheckIfRoomExistByEnglishNameAsync(string englishName);

}