using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;
public class Song
{
    [Required]
    public int? Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public int ArtistId { get; set; }

    [Required]
    public int TimesPlayed { get; set; }
}