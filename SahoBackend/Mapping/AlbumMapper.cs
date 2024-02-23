using SahoBackend.Mapping.Interfaces;
using Domain.DTOs;
using Entities;
using SahoBackend.Models;

namespace SahoBackend.Mapping;

public class AlbumMapper : IAlbumMapper
{
    public AlbumEntity? DtoToEntity(AlbumDto album)
    {
        return album is not null ? new AlbumEntity {
            Id = album.Id.Value,
            Title = album.Title,
            ArtistId = album.ArtistId,
        } : null;
    }

    public AlbumDto? EntityToDto(AlbumEntity album) {
        return album is not null ? new AlbumDto {
            Id = album.Id,
            Title = album.Title,
            ArtistId = album.ArtistId,
        } : null;
    }

    public AlbumDto? ModelToDto(Album album)
    {
        return album is not null ? new AlbumDto {
            Id = album.Id,
            Title = album.Title,
            ArtistId = album.ArtistId,
        } : null;
    }
}