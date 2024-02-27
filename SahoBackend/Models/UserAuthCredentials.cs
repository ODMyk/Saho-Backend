using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;

public class UserAuthCredentials
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$")]
    public string Login { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")] // Minimum eight characters, at least one letter and one number:
    public string Password { get; set; } = null!;
}