using Domain.DTOs;
using Entities;

namespace Domain.Services;
public interface IUserService
{
    Task<UserEntity> RetrieveAsync(int id);

    Task<IList<UserEntity>> GetLikedArtistsAsync(int id);

    Task<IList<SongEntity>> GetLikedSongsAsync(int id);

    Task<IList<AlbumEntity>> GetLikedAlbumsAsync(int id);

    Task<IList<PlaylistEntity>> GetLikedPlaylistsAsync(int id);

    Task<IList<SongEntity>> GetUserSongsAsync(int id);

    Task<IList<SongEntity>> GetArtistSongsAsync(int id);

    Task<IList<PlaylistEntity>> GetPlaylistsAsync(int id);

    Task<IList<UserEntity>> GetAllAsync();
    Task<int> CreateOrUpdateAsync(UserDto user);
    Task<bool> DeleteAsync(int id);

    Task<bool> LikeSong(int id, SongEntity song);

    Task<bool> LikeArtist(int id, UserEntity artist);

    Task<bool> LikeAlbum(int id, AlbumEntity album);

    Task<bool> LikePlaylist(int id, PlaylistEntity playlist);

    Task<bool> UnlikeSong(int id, SongEntity song);

    Task<bool> UnlikeArtist(int id, UserEntity artist);

    Task<bool> UnlikeAlbum(int id, AlbumEntity album);

    Task<bool> UnlikePlaylist(int id, PlaylistEntity playlist);
}