using SahoBackend.Models;
using Infrastructure.SQL.Entities;
using Domain.DTOs;

namespace SahoBackend.Mapping.Interfaces;

public interface IPlaylistMapper
{
    public PlaylistDto? Map(Playlist model);
    public PlaylistDto? Map(PlaylistEntity entity);
}
