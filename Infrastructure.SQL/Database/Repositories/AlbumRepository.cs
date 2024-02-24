using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories;
public class AlbumRepository(PostgreDbContext context) : IAlbumRepository
{
    private readonly PostgreDbContext _context = context;

    public async Task<int> CreateAsync(AlbumDto album)
    {
        var albumEntity = new AlbumEntity
        {
            Title = album.Title,
            ArtistId = album.ArtistId,
        };
        await _context.AddAsync(albumEntity);
        await _context.SaveChangesAsync();

        return albumEntity.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _context.Albums.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<int> UpdateAsync(AlbumDto album)
    {
        var albumEntity = new AlbumEntity
        {
            Id = album.Id.Value,
            Title = album.Title,
            ArtistId = album.ArtistId,
        };

        return await _context.Albums.Where(x => x.Id == albumEntity.Id).ExecuteUpdateAsync(s => s.SetProperty(p => p.Title, albumEntity.Title).SetProperty(p => p.ArtistId, albumEntity.ArtistId));
    }

    public async Task<AlbumEntity> RetrieveAsync(int id)
    {
        return await _context.Albums.AsNoTracking().Where(x => x.Id == id).Select(x => x).FirstOrDefaultAsync();
    }

    public async Task<IList<AlbumEntity>> GetAllAsync()
    {
        return await _context.Albums.AsNoTracking().Select(x => x).ToListAsync();
    }

    public async Task<IList<SongEntity>> GetSongsAsync(int id)
    {
        var album = await _context.Albums.AsNoTracking().Include(a => a.Songs).Where(a => a.Id == id).FirstOrDefaultAsync();
        if (album is null) {
            return null;
        }

        return [.. album.Songs];
    }
}
