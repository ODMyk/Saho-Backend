using System;
using System.Collections.Generic;

namespace Infrastructure.SQL.Database.Entities;

public partial class AlbumEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public virtual UserEntity ArtistEntity { get; set; } = null!;

    public virtual ICollection<SongEntity> Songs { get; set; } = new List<SongEntity>();

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
