using Domain.DTOs;

namespace Domain.Repositories;
public interface IPlaylistRepository
{
    Task<PlaylistDto> RetrieveAsync(int id);
    Task<IList<PlaylistDto>> GetAllAsync();
    Task<int> CreateAsync(PlaylistDto playlist);
    Task<int> UpdateAsync(PlaylistDto playlist);
    Task<int> DeleteAsync(int id);
}