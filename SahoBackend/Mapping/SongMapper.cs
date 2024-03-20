using SahoBackend.Mapping.Interfaces;
using Domain.DTOs;
using Infrastructure.SQL.Entities;
using SahoBackend.Models;

namespace SahoBackend.Mapping;

public class SongMapper : ISongMapper
{
    public SongDto? Map(SongEntity song, bool isLiked = false)
    {
        return song is not null ? new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            ArtistNickname = song.Artist?.Nickname!,
            TimesPlayed = song.TimesPlayed,
            IsLiked = isLiked,
            IsPrivate = song.IsPrivate,
        } : null;
    }

    public SongDto? Map(Song song)
    {
        return song is not null ? new SongDto
        {
            Id = song.Id,
            Title = song.Title,
        } : null;
    }
}