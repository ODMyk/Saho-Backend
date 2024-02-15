using Domain.DTOs;

namespace Domain.Repositories;
public interface ICountryRepository
{
    Task<SongDto> RetrieveAsync(int id);
    Task<List<SongDto>> GetAllAsync();
    Task<int> CreateAsync(SongDto country);
    Task<int> UpdateAsync(SongDto country);
    Task<int> DeleteAsync(int id);
}