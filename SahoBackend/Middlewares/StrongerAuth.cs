using System.Security.Claims;
using SahoBackend.Repositories;

namespace SahoBackend.Middlewares;

public class StrongerAuth(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var repository = context.RequestServices.GetService<IUserRepository>();
        var id = int.Parse(context.User.FindFirst("id")?.Value ?? "-1");
        var user = await repository!.GetUser(id);

        if ((user is null && id >= 0)
            || user is not null
            && user.SecurityCode != int.Parse(context.User.FindFirstValue("SecurityCode")!))
        {
            context.Response.StatusCode = 401;
            return;
        }

        context.Items["User"] = user;

        await _next(context);
    }
}