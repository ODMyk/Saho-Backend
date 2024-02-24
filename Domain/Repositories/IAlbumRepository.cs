using Domain.DTOs;
using Entities;

namespace Domain.Repositories;
public interface IAlbumRepository
{
    Task<AlbumEntity> RetrieveAsync(int id);
    Task<IList<AlbumEntity>> GetAllAsync();
    Task<int> CreateAsync(AlbumDto album);
    Task<int> UpdateAsync(AlbumDto album);
    Task<int> DeleteAsync(int id);

    Task<IList<SongEntity>> GetSongsAsync(int id);
}