using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;

public class UserRegisterCredentials : UserAuthCredentials
{
    [Required]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    public string Email { get; set; } = null!;
}