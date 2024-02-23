using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories;
public class UserRepository : IUserRepository
{
    private readonly PostgreDbContext _context;

    public UserRepository(PostgreDbContext context)
    {
        _context = context;
    }
    public async Task<int> CreateAsync(UserDto user)
    {
        var userEntity = new UserEntity
        {
            Nickname = user.Nickname,
            RoleId = user.RoleId,
        };
        await _context.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        return userEntity.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _context.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<int> UpdateAsync(UserDto user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id.Value,
            Nickname = user.Nickname,
            RoleId = user.RoleId,
        };

        return await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(s => s.SetProperty(p => p.Nickname, userEntity.Nickname).SetProperty(p => p.RoleId, user.RoleId));
    }

    public async Task<UserEntity> RetrieveAsync(int id)
    {
        return await _context.Users.Where(x => x.Id == id).Select(x => x).FirstOrDefaultAsync();
    }

    public async Task<IList<UserEntity>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().Select(x => x).ToListAsync();
    }

    public async Task<bool> LikeSong(int id, SongEntity song)
    {
        var user = await RetrieveAsync(id);
        if (user is null) {
            return false;
        }

        user.LikedSongs.Add(song);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> LikeArtist(int id, UserEntity artist)
    {
        var user = await RetrieveAsync(id);
        if (user is null) {
            return false;
        }

        user.LikedArtists.Add(artist);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> LikeAlbum(int id, AlbumEntity album)
    {
        var user = await RetrieveAsync(id);
        if (user is null) {
            return false;
        }

        user.LikedAlbums.Add(album);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> LikePlaylist(int id, PlaylistEntity playlist)
    {
        var user = await RetrieveAsync(id);
        if (user is null) {
            return false;
        }

        user.LikedPlaylists.Add(playlist);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UnlikeArtist(int id, UserEntity artist)
    {
        var user = await RetrieveAsync(id);
        if (user.LikedArtists.Remove(artist)) {
            return await _context.SaveChangesAsync() > 0;
        }
        return true;
    }

    public async Task<bool> UnlikeSong(int id, SongEntity song)
    {
        var user = await RetrieveAsync(id);
        if (user.LikedSongs.Remove(song)) {
            return await _context.SaveChangesAsync() > 0;
        }
        return true;
    }
    
    public async Task<bool> UnlikeAlbum(int id, AlbumEntity album)
    {
        var user = await RetrieveAsync(id);
        if (user.Albums.Remove(album)) {
            return await _context.SaveChangesAsync() > 0;
        }
        return true;
    }

    public async Task<bool> UnlikePlaylist(int id, PlaylistEntity playlist)
    {
        var user = await RetrieveAsync(id);
        if (user.Playlists.Remove(playlist)) {
            return await _context.SaveChangesAsync() > 0;
        }
        return true;
    }
}
