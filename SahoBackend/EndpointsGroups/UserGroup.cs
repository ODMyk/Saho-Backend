using Microsoft.AspNetCore.Authorization;
using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class UserGroup
{
    public static void AddUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users");

        group.RequireAuthorization(new AuthorizeAttribute { Policy = "UserPolicy" });

        group.MapGet("/{id}", UserEndpoints.GetUserById).WithName("userById");
        group.MapGet("/me/extended", UserEndpoints.GetExtendedInfo);
        group.MapGet("/makeArtist", UserEndpoints.MakeUserArtist);
        group.MapGet("/", UserEndpoints.GetAllUsers);
        group.MapDelete("/{id}", UserEndpoints.DeleteUserById);
        group.MapPatch("/nickname", UserEndpoints.UpdateNickname);
        group.MapPut("/password", UserEndpoints.ChangePassword);
        group.MapPut("/picture", UserEndpoints.UpdateProfilePicture).DisableAntiforgery();
        group.MapDelete("/picture", UserEndpoints.DeleteProfilePicture);
        group.MapGet("/{artistId}/songs", UserEndpoints.GetSongsOfUser);
    }
}