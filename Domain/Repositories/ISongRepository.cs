using Domain.DTOs;
using Entities;

namespace Domain.Repositories;
public interface ISongRepository
{
    Task<SongEntity> RetrieveAsync(int id);
    Task<IList<SongEntity>> GetAllAsync();
    Task<int> CreateAsync(SongDto song);
    Task<int> UpdateAsync(SongDto song);
    Task<int> DeleteAsync(int id);

    Task<bool> AddToAlbum(int id, AlbumEntity album);

    Task<bool> AddToPlaylist(int id, PlaylistEntity playlist);

    Task<bool> RemoveFromAlbum(int id, AlbumEntity album);

    Task<bool> RemoveFromPlaylist(int id, PlaylistEntity playlist);
}