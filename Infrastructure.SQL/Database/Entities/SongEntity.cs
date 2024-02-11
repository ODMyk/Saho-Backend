using System;
using System.Collections.Generic;

namespace Infrastructure.SQL.Database.Entities;

public partial class SongEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public int TimesPlayed { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
