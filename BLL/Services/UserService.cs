using Domain.DTOs;
using Domain.Repositories;
using DOmain.Services;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(id) > 0;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }
    public async Task<UserDto> RetrieveAsync(int id)
    {
        return await _userRepository.RetrieveAsync(id);
    }
    public async Task<int> CreateOrUpdateAsync(UserDto user)
    {
        if (user?.Id is null)
        {
            return await _userRepository.CreateAsync(user);
        }
        if (await _userRepository.CreateAsync(user) > 0)
        {
            return user.Id;
        }
        return 0;
    }
    public async Task<bool> UpdateDescriptionAsync(int id, string description)
    {
        return await _userRepository.UpdateDescriptionAsync(id, description) > 0;
    }