using Domain.DTOs;
using Domain.Repositories;
using DOmain.Services;

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

    public async Task<List<SongDto>> GetAllAsync()
    {
        return await _songRepository.GetAllAsync();
    }
    public async Task<SongDto> RetrieveAsync(int id)
    {
        return await _songRepository.RetrieveAsync(id);
    }
    public async Task<int> CreateOrUpdateAsync(SongDto song)
    {
        if (song?.Id is null)
        {
            return await _songRepository.CreateAsync(song);
        }
        if (await _songRepository.CreateAsync(song) > 0)
        {
            return song.Id;
        }
        return 0;
    }
    public async Task<bool> UpdateDescriptionAsync(int id, string description)
    {
        return await _songRepository.UpdateDescriptionAsync(id, description) > 0;
    }