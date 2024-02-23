using SahoBackend.Models;
using Entities;
using Domain.DTOs;

namespace SahoBackend.Mapping.Interfaces;

public interface IPlaylistMapper
{
    public PlaylistDto? ModelToDto(Playlist model);
    public PlaylistEntity? DtoToEntity(PlaylistDto dto);
    public PlaylistDto? EntityToDto(PlaylistEntity entity);
}
