using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories;
public class SongRepository : ISongRepository
{
    private readonly PostgreDbContext _context;

    public SongRepository(PostgreDbContext context)
    {
        _context = context;
    }
    public async Task<int> CreateAsync(SongDto song)
    {
        var songEntity = new SongEntity
        {
            Title = song.Nickame,
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
            Id = song.Id,
            Title = song.Title,
            TimesPlayed = song.TimesPlayed,
            ArtistId = song.ArtistId,
        };

        return await _context.Songs.Where(x => x.Id == songEntity.Id).executeUpdateAsync(s => s.SetProperty(p => p.Title, songEntity.Title).SetProperty(p => p.TimesPlayed, songEntity.TimesPlayed).SetProperty(p => p.ArtistId, songEntity.ArtistId));
    }

    public async Task<SongDto> RetreiveAsync(int id)
    {
        return await _context.AsNoTracking().Where(x => x.Id == id).Select(x => new SongDto { Id = x.Id, Title = x.Title, TimesPlayed = x.TimesPlayed, ArtistId = x.ArtistId }).FirstOrDefaultAsync();
    }

    public async Task<IList<SongDto>> GetAllAsync()
    {
        return await _context.AsNoTracking().Select(x => new SongDto { Id = x.Id, Title = x.Title, TimesPlayed = x.TimesPlayed, ArtistId = x.ArtistId }).ToListAsync();
    }
}
