using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories;
public class PlaylistRepository(PostgreDbContext context) : IPlaylistRepository
{
    private readonly PostgreDbContext _context = context;

    public async Task<int> CreateAsync(PlaylistDto playlist)
    {
        var playlistEntity = new PlaylistEntity
        {
            Title = playlist.Title,
            IsPrivate = playlist.IsPrivate,
            OwnerId = playlist.OwnerId,
        };
        await _context.AddAsync(playlistEntity);
        await _context.SaveChangesAsync();

        return playlistEntity.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _context.Playlists.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<int> UpdateAsync(PlaylistDto playlist)
    {
        var playlistEntity = new PlaylistEntity
        {
            Id = playlist.Id.Value,
            Title = playlist.Title,
            IsPrivate = playlist.IsPrivate,
            OwnerId = playlist.OwnerId,
        };

        return await _context.Playlists.Where(x => x.Id == playlistEntity.Id).ExecuteUpdateAsync(s => s.SetProperty(p => p.Title, playlistEntity.Title).SetProperty(p => p.IsPrivate, playlistEntity.IsPrivate).SetProperty(p => p.OwnerId, playlistEntity.OwnerId));
    }

    public async Task<PlaylistEntity> RetrieveAsync(int id)
    {
        return await _context.Playlists.AsNoTracking().Where(x => x.Id == id).Select(x => x).FirstOrDefaultAsync();
    }

    public async Task<IList<PlaylistEntity>> GetAllAsync()
    {
        return await _context.Playlists.AsNoTracking().Select(x => x).ToListAsync();
    }

    public async Task<IList<SongEntity>> GetSongsAsync(int id)
    {
        var playlist = await _context.Playlists.AsNoTracking().Include(p => p.Songs).Where(p => p.Id == id).FirstOrDefaultAsync();
        if (playlist is null) {
            return null;
        }

        return [.. playlist.Songs];
    }
}
