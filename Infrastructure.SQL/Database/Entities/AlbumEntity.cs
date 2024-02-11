using System;
using System.Collections.Generic;

namespace Infrastructure.SQL.Database.Entities;

public partial class AlbumEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public virtual User Artist { get; set; } = null!;

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
