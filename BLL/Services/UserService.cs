using Domain.DTOs;
using Domain.Repositories;
using Domain.Services;
using Entities;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(id) > 0;
    }

    public async Task<IList<UserEntity>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }
    public async Task<UserEntity> RetrieveAsync(int id)
    {
        return await _userRepository.RetrieveAsync(id);
    }
    public async Task<int> CreateOrUpdateAsync(UserDto user)
    {
        if (user.Id is null)
        {
            return await _userRepository.CreateAsync(user);
        }
        if (await _userRepository.UpdateAsync(user) > 0)
        {
            return user.Id.Value;
        }
        return -1;
    }

    public async Task<bool> LikeSong(int id, SongEntity song)
    {
        return await _userRepository.LikeSong(id, song);
    }

    public async Task<bool> LikeArtist(int id, UserEntity artist)
    {
        return await _userRepository.LikeArtist(id, artist);
    }

    public async Task<bool> LikeAlbum(int id, AlbumEntity album)
    {
        return await _userRepository.LikeAlbum(id, album);
    }

    public async Task<bool> LikePlaylist(int id, PlaylistEntity playlist)
    {
        return await _userRepository.LikePlaylist(id, playlist);
    }

    public async Task<bool> UnlikeSong(int id, SongEntity song)
    {
        return await _userRepository.UnlikeSong(id, song);
    }

    public async Task<bool> UnlikeArtist(int id, UserEntity artist)
    {
        return await _userRepository.UnlikeArtist(id, artist);
    }

    public async Task<bool> UnlikeAlbum(int id, AlbumEntity album)
    {
        return await _userRepository.UnlikeAlbum(id, album);
    }

    public async Task<bool> UnlikePlaylist(int id, PlaylistEntity playlist)
    {
        return await _userRepository.UnlikePlaylist(id, playlist);
    }
}