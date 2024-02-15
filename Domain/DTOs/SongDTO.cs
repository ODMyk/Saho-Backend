namespace Domain.DTOs;

public class SongDTO
{
    public int? Id { get; set; }
    public string Title { get; set; }
    public int ArtistId { get; set; }
    public int TimesPlayed { get; set; }
}