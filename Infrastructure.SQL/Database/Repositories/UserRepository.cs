using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories;
public class UserRepository(PostgreDbContext context) : IUserRepository
{
    private readonly PostgreDbContext _context = context;

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
        return await _context.Users.AsNoTracking().Where(x => x.Id == id).Select(x => x).FirstOrDefaultAsync();
    }

    public async Task<IList<UserEntity>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().Select(x => x).ToListAsync();
    }

    public async Task<bool> LikeSong(int id, SongEntity song)
    {
        var user = await _context.Users.Include(u => u.LikedSongs).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return false;
        }

        if (user.LikedSongs.Any(s => s.Id == song.Id))
        {
            return true;
        }

        user.LikedSongs.Add(song);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> LikeArtist(int id, UserEntity artist)
    {
        var user = await _context.Users.Include(u => u.LikedArtists).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return false;
        }

        if (user.LikedArtists.Any(u => u.Id == user.Id))
        {
            return true;
        }

        user.LikedArtists.Add(artist);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> LikeAlbum(int id, AlbumEntity album)
    {
        var user = await _context.Users.Include(u => u.LikedAlbums).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return false;
        }

        if (user.LikedAlbums.Any(a => a.Id == album.Id))
        {
            return true;
        }

        // user.LikedAlbums.Add(new AlbumEntity { Id = album.Id, Title = album.Title, ArtistId = album.ArtistId }); // here I'll do refactoring
        user.LikedAlbums.Add(album);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> LikePlaylist(int id, PlaylistEntity playlist)
    {
        var user = await _context.Users.Include(u => u.LikedPlaylists).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return false;
        }

        if (user.LikedPlaylists.Any(p => p.Id == playlist.Id))
        {
            return true;
        }

        user.LikedPlaylists.Add(playlist);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UnlikeArtist(int id, UserEntity artist)
    {
        var user = await _context.Users.Include(u => u.LikedArtists).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return false;
        }

        if (user.LikedArtists.Remove(user.LikedArtists.Where(u => u.Id == artist.Id).FirstOrDefault()))
        {
            return await _context.SaveChangesAsync() > 0;
        }
        return true;
    }

    public async Task<bool> UnlikeSong(int id, SongEntity song)
    {
        var user = await _context.Users.Include(u => u.LikedSongs).Where(u => u.Id == id).FirstOrDefaultAsync();

        if (user is null)
        {
            return false;
        }

        if (user.LikedSongs.Remove(user.LikedSongs.Where(s => s.Id == song.Id).FirstOrDefault()))
        {
            return await _context.SaveChangesAsync() > 0;
        }

        return true;
    }

    public async Task<bool> UnlikeAlbum(int id, AlbumEntity album)
    {
        var user = await _context.Users.Include(u => u.LikedAlbums).Where(u => u.Id == id).FirstOrDefaultAsync();

        if (user is null)
        {
            return false;
        }

        if (user.LikedAlbums.Remove(user.LikedAlbums.Where(a => a.Id == album.Id).FirstOrDefault()))
        {
            return await _context.SaveChangesAsync() > 0;
        }

        return true;
    }

    public async Task<bool> UnlikePlaylist(int id, PlaylistEntity playlist)
    {
        var user = await _context.Users.Include(u => u.LikedPlaylists).Where(u => u.Id == id).FirstOrDefaultAsync();

        if (user is null)
        {
            return false;
        }

        if (user.LikedPlaylists.Remove(user.LikedPlaylists.Where(p => p.Id == playlist.Id).FirstOrDefault()))
        {
            return await _context.SaveChangesAsync() > 0;
        }

        return true;
    }
}
