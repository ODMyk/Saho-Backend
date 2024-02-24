using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories;
public class SongRepository(PostgreDbContext context) : ISongRepository
{
    private readonly PostgreDbContext _context = context;

    public async Task<int> CreateAsync(SongDto song)
    {   
        var songEntity = new SongEntity
        {
            Title = song.Title,
            TimesPlayed = song.TimesPlayed,
            ArtistId = song.ArtistId,
        };
        await _context.AddAsync(songEntity);
        await _context.SaveChangesAsync();

        return songEntity.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _context.Songs.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<int> UpdateAsync(SongDto song)
    {
        var songEntity = new SongEntity
        {
            Id = song.Id.Value,
            Title = song.Title,
            TimesPlayed = song.TimesPlayed,
            ArtistId = song.ArtistId,
        };

        return await _context.Songs.Where(x => x.Id == songEntity.Id).ExecuteUpdateAsync(s => s.SetProperty(p => p.Title, songEntity.Title).SetProperty(p => p.TimesPlayed, songEntity.TimesPlayed).SetProperty(p => p.ArtistId, songEntity.ArtistId));
    }

    public async Task<SongEntity> RetrieveAsync(int id)
    {
        return await _context.Songs.AsNoTracking().Where(x => x.Id == id).Select(x => x).FirstOrDefaultAsync();
    }

    public async Task<IList<SongEntity>> GetAllAsync()
    {
        return await _context.Songs.AsNoTracking().Select(x => x).ToListAsync();
    }

    public async Task<bool> AddToAlbum(int id, AlbumEntity album)
    {
        var song = await _context.Songs.Include(s => s.Albums).Where(s => s.Id == id).FirstOrDefaultAsync();
    
        if (song is null)
        {
            return false;
        }

        song.Albums.Add(album);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddToPlaylist(int id, PlaylistEntity playlist)
    {
        var song = await _context.Songs.Include(s => s.Playlists).Where(s => s.Id == id).FirstOrDefaultAsync();;
        if (song is null)
        {
            return false;
        }

        song.Playlists.Add(playlist);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveFromAlbum(int id, AlbumEntity album)
    {
        var song = await _context.Songs.Include(s => s.Albums).Where(s => s.Id == id).FirstOrDefaultAsync();
        
        foreach (var a in song.Albums)
        {
            if (a.Id == album.Id) {
                song.Albums.Remove(a);
                return await _context.SaveChangesAsync() > 0;
            }
        }
        
        return true;
    }

    public async Task<bool> RemoveFromPlaylist(int id, PlaylistEntity playlist)
    {
        var song = await RetrieveAsync(id);
        if (song.Playlists.Remove(playlist))
        {
            return await _context.SaveChangesAsync() > 0;
        }
        
        return true;
    }
}
