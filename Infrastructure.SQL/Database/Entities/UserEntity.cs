using System;
using System.Collections.Generic;

namespace Infrastructure.SQL.Database.Entities;

public partial class UserEntity
{
    public int Id { get; set; }

    public string Nickname { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Playlist> PlaylistsNavigation { get; set; } = new List<Playlist>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Album> AlbumsNavigation { get; set; } = new List<Album>();

    public virtual ICollection<User> Artists { get; set; } = new List<User>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
