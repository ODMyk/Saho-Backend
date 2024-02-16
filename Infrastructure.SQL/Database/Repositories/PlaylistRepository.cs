using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories;
public class PlaylistRepository : IPlaylistRepository
{
    private readonly PostgreDbContext _context;

    public PlaylistRepository(PostgreDbContext context)
    {
        _context = context;
    }
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

    public async Task<PlaylistDto> RetrieveAsync(int id)
    {
        return await _context.Playlists.AsNoTracking().Where(x => x.Id == id).Select(x => new PlaylistDto { Id = x.Id, Title = x.Title, IsPrivate = x.IsPrivate, OwnerId = x.OwnerId }).FirstOrDefaultAsync();
    }

    public async Task<IList<PlaylistDto>> GetAllAsync()
    {
        return await _context.Playlists.AsNoTracking().Select(x => new PlaylistDto { Id = x.Id, Title = x.Title, IsPrivate = x.IsPrivate, OwnerId = x.OwnerId }).ToListAsync();
    }
}
