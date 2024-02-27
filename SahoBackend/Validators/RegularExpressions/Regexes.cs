using System.Text.RegularExpressions;

namespace SahoBackend.Validators.RegularExpressions;

public static class Regexes
{
    public static string HTML_Tag { get; } = "<.*?>";

    public static string Email { get; } = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

    // Minimum eight characters, at least one letter and one number
    public static string Password { get; } = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";

    public static string Title { get; } = @"^[A-Za-z0-9\s\-_,\.&:;()''""]+$";

    public static string Login { get; } = "^[a-zA-Z0-9]+$";
}