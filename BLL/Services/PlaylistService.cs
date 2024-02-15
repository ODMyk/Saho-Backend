using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;

namespace BLL.Services;

public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository;
    public PlaylistService(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        return await _playlistRepository.DeleteAsync(id) > 0;
    }

    public async Task<IList<PlaylistDto>> GetAllAsync()
    {
        return await _playlistRepository.GetAllAsync();
    }
    public async Task<PlaylistDto> RetrieveAsync(int id)
    {
        return await _playlistRepository.RetrieveAsync(id);
    }
    public async Task<int> CreateOrUpdateAsync(PlaylistDto playlist)
    {
        if (playlist.Id is null)
        {
            return await _playlistRepository.CreateAsync(playlist);
        }
        if (await _playlistRepository.CreateAsync(playlist) > 0)
        {
            return playlist.Id.Value;
        }
        return 0;
    }
}