using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;

public class User
{
    [Required]
    public int? Id { get; set; }

    [Required]
    public string Nickname { get; set; }
}