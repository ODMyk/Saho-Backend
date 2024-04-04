using System;
using System.Collections.Generic;

namespace Infrastructure.SQL.Entities;

public partial class SongEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int AlbumId { get; set; }

    public int TimesPlayed { get; set; }

    public bool HasLyrics { get; set; }

    public virtual AlbumEntity Album { get; set; } = null!;

    public virtual ICollection<PlaylistEntity> Playlists { get; set; } = new List<PlaylistEntity>();

    public virtual ICollection<ArtistEntity> LikedBy { get; set; } = new List<ArtistEntity>();
}
