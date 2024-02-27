using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;

public class User
{
    [Required]
    public int? Id { get; set; }

    [Required]
    [RegularExpression(@"^[A-Za-z0-9\s\-_,\.&:;()''""]+$")]
    public string Nickname { get; set; }
}