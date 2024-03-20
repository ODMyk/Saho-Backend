using SahoBackend.Models;
using Infrastructure.SQL.Entities;
using Domain.DTOs;

namespace SahoBackend.Mapping.Interfaces;

public interface IAlbumMapper
{
    public AlbumDto? Map(Album model);
    public AlbumDto? Map(AlbumEntity entity);
}
