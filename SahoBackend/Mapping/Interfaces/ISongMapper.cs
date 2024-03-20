using SahoBackend.Models;
using Infrastructure.SQL.Entities;
using Domain.DTOs;

namespace SahoBackend.Mapping.Interfaces;

public interface ISongMapper
{
    public SongDto? Map(Song model);
    public SongDto? Map(SongEntity entity, bool isLiked = false);
}
