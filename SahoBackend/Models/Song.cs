using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;
public class Song
{
    [Required]
    public int? Id { get; set; }

    [Required]
    [RegularExpression(@"^[\p{L}\p{N}\s-_,\.&:;()'""]+$")]
    public string Title { get; set; }

    [Required]
    public bool HasLyrics { get; set; }

    [Required]
    public int AlbumId { get; set; }
}