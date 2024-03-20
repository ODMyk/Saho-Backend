using SahoBackend.Mapping.Interfaces;
using Domain.DTOs;
using Infrastructure.SQL.Entities;
using SahoBackend.Models;

namespace SahoBackend.Mapping;

public class PlaylistMapper : IPlaylistMapper
{
    public PlaylistDto? Map(PlaylistEntity playlist) {
        return playlist is not null ? new PlaylistDto {
            Id = playlist.Id,
            Title = playlist.Title,
            OwnerNickname = playlist.Owner.Nickname,
            IsPrivate = playlist.IsPrivate,
        } : null;
    }

    public PlaylistDto? Map(Playlist playlist)
    {
        return playlist is not null ? new PlaylistDto {
            Id = playlist.Id,
            Title = playlist.Title,
            IsPrivate = playlist.IsPrivate,
        } : null;
    }
}