using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using Entities;

namespace BLL.Services;

public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;
    public SongService(ISongRepository songRepository)
    {
        _songRepository = songRepository;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        return await _songRepository.DeleteAsync(id) > 0;
    }

    public async Task<IList<SongEntity>> GetAllAsync()
    {
        return await _songRepository.GetAllAsync();
    }
    public async Task<SongEntity> RetrieveAsync(int id)
    {
        return await _songRepository.RetrieveAsync(id);
    }
    public async Task<int> CreateOrUpdateAsync(SongDto song)
    {
        if (song.Id is null)
        {
            return await _songRepository.CreateAsync(song);
        }
        if (await _songRepository.UpdateAsync(song) > 0)
        {
            return song.Id.Value;
        }
        return -1;
    }

    public Task<bool> AddToAlbum(int id, AlbumEntity album)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddToPlaylist(int id, PlaylistEntity playlist)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveFromAlbum(int id, AlbumEntity album)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveFromPlaylist(int id, PlaylistEntity playlist)
    {
        throw new NotImplementedException();
    }
}