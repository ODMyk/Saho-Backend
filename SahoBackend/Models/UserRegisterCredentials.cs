namespace SahoBackend.Models;

public class UserRegisterCredentials : UserAuthCredentials
{
    public string Nickname { get; set; } = null!;

    public string Email { get; set; } = null!;
}