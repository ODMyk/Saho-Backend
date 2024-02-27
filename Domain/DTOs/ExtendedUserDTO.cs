namespace Domain.DTOs;

public class ExtendedUserDTO : UserDto
{
    public string Email { get; set; } = null!;

    public string Login { get; set; } = null!;
}