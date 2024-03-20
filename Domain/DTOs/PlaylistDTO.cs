namespace Domain.DTOs;

public class PlaylistDto
{
    public int? Id { get; set; }
    public string Title { get; set; } = null!;
    public string OwnerNickname { get; set; } = null!;
    public bool IsPrivate { get; set; }
}