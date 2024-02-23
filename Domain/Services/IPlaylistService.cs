using Domain.DTOs;
using Entities;
namespace Domain.Services;
public interface IPlaylistService
{
    Task<PlaylistEntity> RetrieveAsync(int id);
    Task<IList<PlaylistEntity>> GetAllAsync();
    Task<int> CreateOrUpdateAsync(PlaylistDto playlist);
    Task<bool> DeleteAsync(int id);
}