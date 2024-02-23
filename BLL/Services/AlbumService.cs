using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using Entities;

namespace BLL.Services;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    public AlbumService(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        return await _albumRepository.DeleteAsync(id) > 0;
    }

    public async Task<IList<AlbumEntity>> GetAllAsync()
    {
        return await _albumRepository.GetAllAsync();
    }
    public async Task<AlbumEntity> RetrieveAsync(int id)
    {
        return await _albumRepository.RetrieveAsync(id);
    }
    public async Task<int> CreateOrUpdateAsync(AlbumDto album)
    {
        if (album.Id is null)
        {
            return await _albumRepository.CreateAsync(album);
        }
        
        if (await _albumRepository.UpdateAsync(album) > 0)
        {
            return album.Id.Value;
        }
        return 0;
    }
}
