using Microsoft.AspNetCore.Authorization;
using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class UserGroup
{
    public static void AddUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users");

        group.RequireAuthorization(new AuthorizeAttribute { Policy = "UserPolicy" });

        group.MapGet("/me/extended", UserEndpoints.GetExtendedInfo);
        group.MapGet("/makeArtist", UserEndpoints.MakeUserOrganization);
        group.MapPut("/password", UserEndpoints.ChangePassword);
        group.MapGet("/", UserEndpoints.GetAllUsers).RequireAuthorization(new AuthorizeAttribute { Policy = "AdminPolicy" });
        group.MapDelete("/{id}", UserEndpoints.DeleteUserById).RequireAuthorization(new AuthorizeAttribute { Policy = "AdminPolicy" });
    }
}