namespace Domain.DTOs;
public class SongDto
{
    public int? Id { get; set; }
    public string Title { get; set; } = null!;
    public string ArtistNickname { get; set; } = null!;
    public int TimesPlayed { get; set; }

    public bool IsLiked { get; set; }

    public bool IsPrivate { get; set; }
}