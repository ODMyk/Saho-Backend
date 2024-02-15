using Domain.DTOs;

namespace Domain.Repositories;
public interface ISongRepository
{
    Task<SongDto> RetrieveAsync(int id);
    Task<IList<SongDto>> GetAllAsync();
    Task<int> CreateAsync(SongDto song);
    Task<int> UpdateAsync(SongDto song);
    Task<int> DeleteAsync(int id);
}