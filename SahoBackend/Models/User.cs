using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;

public class User
{
    [Required]
    [RegularExpression(@"^[\p{L}\p{N}\s-_,\.&:;()'""]+$")]
    public string Nickname { get; set; }

    [Required]
    public bool HasProfilePicture { get; set; }
}