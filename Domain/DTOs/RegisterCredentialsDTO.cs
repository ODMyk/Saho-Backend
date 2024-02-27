namespace Domain.DTOs;

public class RegisterCredentialsDTO : AuthCredentialsDTO
{
    public string Nickname { get; set; } = null!;

    public string Email { get; set; } = null!;
}