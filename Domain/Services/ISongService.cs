using Domain.DTOs;
using Entities;
namespace Domain.Services;
public interface ISongService
{
    Task<SongEntity> RetrieveAsync(int id);
    Task<IList<SongEntity>> GetAllAsync();
    Task<int> CreateOrUpdateAsync(SongDto song);
    Task<bool> DeleteAsync(int id);

    Task<bool> AddToAlbum(int id, AlbumEntity album);

    Task<bool> AddToPlaylist(int id, PlaylistEntity playlist);

    Task<bool> RemoveFromAlbum(int id, AlbumEntity album);

    Task<bool> RemoveFromPlaylist(int id, PlaylistEntity playlist);
}