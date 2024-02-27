using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;

public class UserRegisterCredentials : UserAuthCredentials
{
    [Required]
    [RegularExpression(@"^[A-Za-z0-9\s\-_,\.&:;()''""]+$")]
    public string Nickname { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    public string Email { get; set; } = null!;
}