using Domain.DTOs;
using Entities;

namespace Domain.Repositories;
public interface IPlaylistRepository
{
    Task<PlaylistEntity> RetrieveAsync(int id);
    Task<IList<PlaylistEntity>> GetAllAsync();
    Task<int> CreateAsync(PlaylistDto playlist);
    Task<int> UpdateAsync(PlaylistDto playlist);
    Task<int> DeleteAsync(int id);
}