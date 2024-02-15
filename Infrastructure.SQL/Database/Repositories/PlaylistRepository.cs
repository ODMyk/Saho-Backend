using Domain.DTOs;

namespace Domain.Repositories;
public interface ICountryRepository
{
    Task<PlaylistDto> RetrieveAsync(int id);
    Task<List<PlaylistDto>> GetAllAsync();
    Task<int> CreateAsync(PlaylistDto country);
    Task<int> UpdateAsync(PlaylistDto country);
    Task<int> DeleteAsync(int id);
}