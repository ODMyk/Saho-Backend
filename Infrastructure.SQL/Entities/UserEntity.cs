namespace Infrastructure.SQL.Entities;

public partial class UserEntity
{
    public int Id { get; set; }

    public string Nickname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int SecurityCode { get; set; }

    public bool HasProfilePicture { get; set; }

    public virtual ICollection<RoleEntity> Roles { get; set; } = [];

    public virtual ICollection<UserEntity> LikedByUsers { get; set; } = [];

    public virtual ICollection<UserEntity> LikedArtists { get; set; } = [];

    public virtual ICollection<SongEntity> Songs { get; set; } = [];

    public virtual ICollection<SongEntity> LikedSongs { get; set; } = [];

    public virtual ICollection<AlbumEntity> Albums { get; set; } = [];

    public virtual ICollection<AlbumEntity> LikedAlbums { get; set; } = [];

    public virtual ICollection<PlaylistEntity> Playlists { get; set; } = [];

    public virtual ICollection<PlaylistEntity> LikedPlaylists { get; set; } = [];
}
