using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Infrastructure.SQL.Repositories;
public class UserRepository(PostgreDbContext context) : IUserRepository
{
    private readonly PostgreDbContext _context = context;

    public async Task<int> DeleteAsync(int id)
    {
        return await _context.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<int> UpdateAsync(ExtendedUserDTO user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id.Value,
            Nickname = user.Nickname,

        };

        return await _context.Users.Where(x => x.Id == user.Id).ExecuteUpdateAsync(s => s.SetProperty(p => p.Nickname, userEntity.Nickname));
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

    public async Task<IList<UserEntity>> GetLikedArtistsAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().Include(u => u.LikedArtists).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }

        return [.. user.LikedArtists];
    }

    public async Task<IList<SongEntity>> GetLikedSongsAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().Include(u => u.LikedSongs).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }

        return [.. user.LikedSongs];
    }

    public async Task<IList<AlbumEntity>> GetLikedAlbumsAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().Include(u => u.LikedAlbums).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }

        return [.. user.LikedAlbums];
    }

    public async Task<IList<PlaylistEntity>> GetLikedPlaylistsAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().Include(u => u.LikedPlaylists).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }

        return [.. user.LikedPlaylists];
    }

    public async Task<IList<SongEntity>> GetUserSongsAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().Include(u => u.Songs).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }

        return [.. user.Songs];
    }

    public async Task<IList<SongEntity>> GetArtistSongsAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().Include(u => u.Songs).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }

        return [.. user.Songs];
    }

    public async Task<IList<PlaylistEntity>> GetPlaylistsAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().Include(u => u.Playlists).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return null;
        }

        return [.. user.Playlists];
    }

    public async Task<UserEntity> FindByLogin(string login)
    {
        return await _context.Users.AsNoTracking().Include(u => u.Roles).Where(u => u.Login == login || u.Email == login).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(RegisterCredentialsDTO dto)
    {
        var user = new UserEntity
        {
            Nickname = dto.Nickname,
            Email = dto.Email,
            Login = dto.Login,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            ProfilePicture = $"http://{Configuration.SahoConfig.Host}/storage/{dto.Nickname}/profile.png"
        };
        await _context.Users.AddAsync(user);
        user.Roles.Add(await _context.Roles.Where(x => x.Id == 1).FirstOrDefaultAsync());
        await _context.SaveChangesAsync();
    }
}
