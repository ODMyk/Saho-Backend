using System;
using System.Collections.Generic;

namespace Entities;

public partial class UserEntity
{
    public int Id { get; set; }

    public string Nickname { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual RoleEntity Role { get; set; } = null!;

    public virtual ICollection<UserEntity> LikedByUsers { get; set; } = new List<UserEntity>();

    public virtual ICollection<UserEntity> LikedArtists { get; set; } = new List<UserEntity>();
    
    public virtual ICollection<SongEntity> Songs { get; set; } = new List<SongEntity>();

    public virtual ICollection<SongEntity> LikedSongs { get; set; } = new List<SongEntity>();

    public virtual ICollection<AlbumEntity> Albums { get; set; } = new List<AlbumEntity>();

    public virtual ICollection<AlbumEntity> LikedAlbums { get; set; } = new List<AlbumEntity>();

    public virtual ICollection<PlaylistEntity> Playlists { get; set; } = new List<PlaylistEntity>();

    public virtual ICollection<PlaylistEntity> LikedPlaylists { get; set; } = new List<PlaylistEntity>();
}
