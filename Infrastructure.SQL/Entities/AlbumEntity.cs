namespace Infrastructure.SQL.Entities;

public partial class AlbumEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public bool HasCover { get; set; }

    public bool IsPrivate { get; set; }

    public virtual ArtistEntity Artist { get; set; } = null!;

    public virtual ICollection<SongEntity> Songs { get; set; } = [];

    public virtual ICollection<ArtistEntity> LikedBy { get; set; } = [];
}
