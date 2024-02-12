using System;
using System.Collections.Generic;

namespace Infrastructure.SQL.Database.Entities;

public partial class UserEntity
{
    public int Id { get; set; }

    public string Nickname { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<AlbumEntity> Albums { get; set; } = new List<AlbumEntity>();

    public virtual ICollection<PlaylistEntity> PlaylistsNavigation { get; set; } = new List<PlaylistEntity>();

    public virtual RoleEntity Role { get; set; } = null!;

    public virtual ICollection<AlbumEntity> AlbumsNavigation { get; set; } = new List<AlbumEntity>();

    public virtual ICollection<UserEntity> Artists { get; set; } = new List<UserEntity>();

    public virtual ICollection<PlaylistEntity> Playlists { get; set; } = new List<PlaylistEntity>();

    public virtual ICollection<SongEntity> Songs { get; set; } = new List<SongEntity>();

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
