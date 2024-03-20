using Infrastructure.SQL.Entities;

namespace SahoBackend.Middlewares;

public class InjectIsAdmin(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        context.Items["IsUserAdmin"] = context.User.IsInRole("Admin") || context.User.IsInRole("Superadmin");

        await _next(context);
    }
}