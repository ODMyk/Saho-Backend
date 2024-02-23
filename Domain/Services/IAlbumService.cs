using Domain.DTOs;
using Entities;
namespace Domain.Services;
public interface IAlbumService
{
    Task<AlbumEntity> RetrieveAsync(int id);
    Task<IList<AlbumEntity>> GetAllAsync();
    Task<int> CreateOrUpdateAsync(AlbumDto album);
    Task<bool> DeleteAsync(int id);
}