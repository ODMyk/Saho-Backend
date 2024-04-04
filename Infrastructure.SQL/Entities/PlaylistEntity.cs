using System;
using System.Collections.Generic;

namespace Infrastructure.SQL.Entities;

public partial class PlaylistEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int OwnerId { get; set; }

    public bool IsPrivate { get; set; }

    public bool HasCover { get; set; }

    public virtual ArtistEntity Owner { get; set; } = null!;

    public virtual ICollection<SongEntity> Songs { get; set; } = [];

    public virtual ICollection<ArtistEntity> LikedBy { get; set; } = [];
}
