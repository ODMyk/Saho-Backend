using SahoBackend.Mapping.Interfaces;
using Domain.DTOs;
using Infrastructure.SQL.Entities;
using SahoBackend.Models;

namespace SahoBackend.Mapping;

public class AlbumMapper : IAlbumMapper
{
    public AlbumDto? Map(AlbumEntity album) {
        return album is not null ? new AlbumDto {
            Id = album.Id,
            Title = album.Title,
            ArtistNickname = album.Artist.Nickname,
        } : null;
    }

    public AlbumDto? Map(Album album)
    {
        return album is not null ? new AlbumDto {
            Id = album.Id,
            Title = album.Title,
        } : null;
    }
}