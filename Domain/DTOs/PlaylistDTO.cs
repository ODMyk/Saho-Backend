namespace Domain.DTOs;

public class PlaylistDto
{
    public int? Id { get; set; }
    public string Title { get; set; }
    public int OwnerId { get; set; }
    public bool IsPrivate { get; set; }
}