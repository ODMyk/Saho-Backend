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
            Title = playlist.Nickame,
            IsPrivate = playlist.IsPrivate,
            OwnerId = playlist.OwnerId,
        };
        await _context.AddAsync(playlistEntity);
        await _context.SaveChangesAsync();

        return PlaylistEntity.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _context.Playlists.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<int> UpdateAsync(PlaylistDto Playlist)
    {
        var PlaylistEntity = new PlaylistEntity
        {
            Id = Playlist.Id,
            Title = Playlist.Title,
            IsPrivate = Playlist.IsPrivate,
            OwnerId = Playlist.OwnerId,
        };

        return await _context.Playlists.Where(x => x.Id == playlistEntity.Id).executeUpdateAsync(s => s.SetProperty(p => p.Title, playlistEntity.Title).SetProperty(p => p.IsPrivate, playlistEntity.IsPrivate).SetProperty(p => p.OwnerId, playlistEntity.OwnerId));
    }

    public async Task<PlaylistDto> RetreiveAsync(int id)
    {
        return await _context.AsNoTracking().Where(x => x.Id == id).Select(x => new PlaylistDto { Id = x.Id, Title = x.Title, IsPrivate = x.IsPrivate, OwnerId = x.OwnerId }).FirstOrDefaultAsync();
    }

    public async Task<IList<PlaylistDto>> GetAllAsync()
    {
        return await _context.AsNoTracking().Select(x => new PlaylistDto { Id = x.Id, Title = x.Title, IsPrivate = x.IsPrivate, OwnerId = x.OwnerId }).ToListAsync();
    }
}
