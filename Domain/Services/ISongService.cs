using Domain.DTOs;
namespace Domain.Services;
public interface ISongService
{
    Task<SongDto> RetrieveAsync(int id);
    Task<IList<SongDto>> GetAllAsync();
    Task<int> CreateOrUpdateAsync(SongDto song);
    Task<bool> DeleteAsync(int id);
}