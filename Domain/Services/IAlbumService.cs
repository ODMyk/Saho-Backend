using Domain.DTOs;
namespace Domain.Services;
public interface IAlbumService
{
    Task<AlbumDto> RetrieveAsync(int id);
    Task<IList<AlbumDto>> GetAllAsync();
    Task<int> CreateOrUpdateAsync(AlbumDto album);
    Task<bool> DeleteAsync(int id);
}