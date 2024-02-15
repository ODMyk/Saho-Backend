using Domain.DTOs;

namespace Domain.Repositories;
public interface IAlbumRepository
{
    Task<AlbumDto> RetrieveAsync(int id);
    Task<List<AlbumDto>> GetAllAsync();
    Task<int> CreateAsync(AlbumDto album);
    Task<int> UpdateAsync(AlbumDto album);
    Task<int> DeleteAsync(int id);
}