using System;
using System.Collections.Generic;

namespace Entities;

public partial class AlbumEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public virtual UserEntity Artist { get; set; } = null!;

    public virtual ICollection<SongEntity> Songs { get; set; } = new List<SongEntity>();

    public virtual ICollection<UserEntity> LikedByUsers { get; set; } = new List<UserEntity>();
}
