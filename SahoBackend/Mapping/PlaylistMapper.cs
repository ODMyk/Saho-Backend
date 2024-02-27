using SahoBackend.Mapping.Interfaces;
using Domain.DTOs;
using Entities;
using SahoBackend.Models;

namespace SahoBackend.Mapping;

public class PlaylistMapper : IPlaylistMapper
{
    public PlaylistEntity? DtoToEntity(PlaylistDto playlist)
    {
        return playlist is not null ? new PlaylistEntity {
            Id = playlist.Id.Value,
            Title = playlist.Title,
            OwnerId = playlist.OwnerId,
            IsPrivate = playlist.IsPrivate,
        } : null;
    }

    public PlaylistDto? EntityToDto(PlaylistEntity playlist) {
        return playlist is not null ? new PlaylistDto {
            Id = playlist.Id,
            Title = playlist.Title,
            OwnerId = playlist.OwnerId,
            IsPrivate = playlist.IsPrivate,
        } : null;
    }

    public PlaylistDto? ModelToDto(Playlist playlist)
    {
        return playlist is not null ? new PlaylistDto {
            Id = playlist.Id,
            Title = playlist.Title,
            IsPrivate = playlist.IsPrivate,
        } : null;
    }
}