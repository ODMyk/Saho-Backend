using SahoBackend.Mapping.Interfaces;
using Domain.DTOs;
using Entities;
using SahoBackend.Models;

namespace SahoBackend.Mapping;

public class SongMapper : ISongMapper
{
    public SongEntity? DtoToEntity(SongDto song)
    {
        return song is not null ? new SongEntity {
            Id = song.Id.Value,
            Title = song.Title,
            ArtistId = song.ArtistId,
            TimesPlayed = song.TimesPlayed,
        } : null;
    }

    public SongDto? EntityToDto(SongEntity song) {
        return song is not null ? new SongDto {
            Id = song.Id,
            Title = song.Title,
            ArtistId = song.ArtistId,
            TimesPlayed = song.TimesPlayed,
        } : null;
    }

    public SongDto? ModelToDto(Song song)
    {
        return song is not null ? new SongDto {
            Id = song.Id,
            Title = song.Title,
        } : null;
    }
}