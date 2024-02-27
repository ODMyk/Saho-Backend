namespace Configuration;

public static class SahoConfig
{
    public static readonly string PostgreConnectionString;
    
    public static readonly string JwtSecret;

    public static readonly string Host;

    static SahoConfig() {
        PostgreConnectionString = Environment.GetEnvironmentVariable("PostgreConnectionString") ?? "";
        JwtSecret = Environment.GetEnvironmentVariable("JwtSecret") ?? "";
        Host = Environment.GetEnvironmentVariable("Host") ?? "";
    }
}
