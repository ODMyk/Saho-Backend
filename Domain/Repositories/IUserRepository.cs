using Domain.DTOs;

namespace Domain.Repositories;
public interface IUserRepository
{
    Task<UserDto> RetrieveAsync(int id);
    Task<IList<UserDto>> GetAllAsync();
    Task<int> CreateAsync(UserDto user);
    Task<int> UpdateAsync(UserDto user);
    Task<int> DeleteAsync(int id);
}