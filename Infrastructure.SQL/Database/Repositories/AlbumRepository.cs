using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories;
public class AlbumRepository : IAlbumRepository
{
    private readonly PostgreDbContext _context;

    public AlbumRepository(PostgreDbContext context)
    {
        _context = context;
    }
    public async Task<int> CreateAsync(AlbumDto album)
    {
        var albumEntity = new AlbumEntity
        {
            Title = album.Title,
            ArtistId = Album.ArtistId,
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
            Id = album.Id,
            Title = album.Title,
            ArtistId = album.ArtistId,
        };

        return await _context.Albums.Where(x => x.Id == albumEntity.Id).executeUpdateAsync(s => s.SetProperty(p => p.Title, albumEntity.Title).SetProperty(p => p.ArtistId, albumEntity.ArtistId));
    }

    public async Task<AlbumDto> RetreiveAsync(int id)
    {
        return await _context.AsNoTracking().Where(x => x.Id == id).Select(x => new AlbumDto { Id = x.Id, Title = x.Title, ArtistId = x.ArtistId }).FirstOrDefaultAsync();
    }

    public async Task<IList<AlbumDto>> GetAllAsync()
    {
        return await _context.AsNoTracking().Select(x => new AlbumDto { Id = x.Id, Title = x.Title, ArtistId = x.ArtistId }).ToListAsync();
    }
}
