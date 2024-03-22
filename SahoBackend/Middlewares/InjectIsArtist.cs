using Infrastructure.SQL.Entities;

namespace SahoBackend.Middlewares;

public class InjectIsArtist(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        context.Items["IsUserArtist"] = context.User.IsInRole("Artist");

        await _next(context);
    }
}