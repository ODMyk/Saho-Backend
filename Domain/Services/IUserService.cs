using Domain.DTOs;
namespace Domain.Services;
public interface IUserService
{
    Task<UserDto> RetrieveAsync(int id);
    Task<IList<UserDto>> GetAllAsync();
    Task<int> CreateOrUpdateAsync(UserDto user);
    Task<bool> DeleteAsync(int id);
}