using Domain.DTOs;
using Entities;

namespace Domain.Repositories;
public interface IUserRepository
{
    Task<UserEntity> RetrieveAsync(int id);
    Task<IList<UserEntity>> GetAllAsync();
    Task<int> CreateAsync(UserDto user);
    Task<int> UpdateAsync(UserDto user);
    Task<int> DeleteAsync(int id);

    Task<bool> LikeSong(int id, SongEntity song);

    Task<bool> LikeArtist(int id, UserEntity artist);

    Task<bool> LikeAlbum(int id, AlbumEntity album);

    Task<bool> LikePlaylist(int id, PlaylistEntity playlist);

    Task<bool> UnlikeSong(int id, SongEntity song);

    Task<bool> UnlikeArtist(int id, UserEntity artist);

    Task<bool> UnlikeAlbum(int id, AlbumEntity album);

    Task<bool> UnlikePlaylist(int id, PlaylistEntity playlist);
}