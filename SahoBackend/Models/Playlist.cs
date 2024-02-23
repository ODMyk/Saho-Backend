using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;
public class Playlist
{
    [Required]
    public int? Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public int OwnerId { get; set; }

    [Required]
    public bool IsPrivate { get; set; }
}