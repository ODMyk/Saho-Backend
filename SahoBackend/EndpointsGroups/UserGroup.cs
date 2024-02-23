using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class UserGroup
{
    public static void AddEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users");

        group.MapGet("/{id}", UserEndpoints.GetUserById).WithName("userById");
        group.MapPost("/", UserEndpoints.PostUser);
        group.MapPut("/", UserEndpoints.PutUser);
        group.MapDelete("/{id}", UserEndpoints.DeleteUserById);
        group.MapGet("/", UserEndpoints.GetAllUsers);
        group.MapPost("/favourites/songs/{songId}", UserEndpoints.AddSongToFavourites);
        group.MapPost("/favourites/artists/{artistId}", UserEndpoints.AddArtistToFavourites);
        group.MapPost("/favourites/playlists/{playlistId}", UserEndpoints.AddPlaylistToFavourites);
        group.MapPost("/favourites/albums/{albumId}", UserEndpoints.AddAlbumToFavourites);
        group.MapDelete("/favourites/songs/{songId}", UserEndpoints.RemoveSongFromFavourites);
        group.MapDelete("/favourites/artists/{artistId}", UserEndpoints.RemoveArtistFromFavourites);
        group.MapDelete("/favourites/playlists/{playlistId}", UserEndpoints.RemovePlaylistFromFavourites);
        group.MapDelete("/favourites/albums/{albumId}", UserEndpoints.RemoveAlbumFromFavourites);
    }
}