namespace Domain.DTOs;

public class AlbumDto
{
    public int? Id { get; set; }
    public string Title { get; set; } = null!;
    public string ArtistNickname { get; set; } = null!;
}