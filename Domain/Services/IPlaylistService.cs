using Domain.DTOs;
namespace Domain.Services;
public interface IPlaylistService
{
    Task<PlaylistDto> RetrieveAsync(int id);
    Task<IList<PlaylistDto>> GetAllAsync();
    Task<int> CreateOrUpdateAsync(PlaylistDto playlist);
    Task<bool> DeleteAsync(int id);
}