namespace Infrastructure.SQL.Entities;

public partial class ArtistEntity
{
    public int Id { get; set; }

    public string Nickname { get; set; } = null!;

    public bool IsHidden { get; set; }

    public int OwnerId { get; set; }

    public virtual UserEntity Owner { get; set; } = null!;

    public virtual ICollection<UserEntity> LikedArtists { get; set; } = [];

    public virtual ICollection<UserEntity> LikedBy { get; set; } = [];

    public virtual ICollection<AlbumEntity> Albums { get; set; } = [];

    public virtual ICollection<PlaylistEntity> Playlists { get; set; } = [];

    public virtual ICollection<PlaylistEntity> LikedPlaylists { get; set; } = [];

    public virtual ICollection<AlbumEntity> LikedAlbums { get; set; } = [];
}