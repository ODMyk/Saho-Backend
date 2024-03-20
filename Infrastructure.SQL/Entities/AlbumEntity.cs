namespace Infrastructure.SQL.Entities;

public partial class AlbumEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public bool HasCover { get; set; }

    public virtual UserEntity Artist { get; set; } = null!;

    public virtual ICollection<SongEntity> Songs { get; set; } = [];

    public virtual ICollection<UserEntity> LikedByUsers { get; set; } = [];
}
