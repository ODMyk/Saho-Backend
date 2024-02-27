using Microsoft.AspNetCore.Authorization;
using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class UserGroup
{
    public static void AddEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users");

        group.MapGet("/{id}", UserEndpoints.GetUserById).WithName("userById");
        group.MapPut("/", UserEndpoints.PutUser);
        group.MapDelete("/{id}", UserEndpoints.DeleteUserById);
        group.MapGet("/", UserEndpoints.GetAllUsers).RequireAuthorization(new AuthorizeAttribute {Policy = "UserPolicy"});
        group.MapPost("/{userId}/favourites/songs", UserEndpoints.AddSongToFavourites);
        group.MapPost("/{userId}/favourites/artists", UserEndpoints.AddArtistToFavourites);
        group.MapPost("/{userId}/favourites/playlists", UserEndpoints.AddPlaylistToFavourites);
        group.MapPost("/{userId}/favourites/albums", UserEndpoints.AddAlbumToFavourites);
        group.MapDelete("/{userId}/favourites/songs", UserEndpoints.RemoveSongFromFavourites);
        group.MapDelete("/{userId}/favourites/artists", UserEndpoints.RemoveArtistFromFavourites);
        group.MapDelete("/{userId}/favourites/playlists/{playlistId}", UserEndpoints.RemovePlaylistFromFavourites);
        group.MapDelete("/{userId}/favourites/albums", UserEndpoints.RemoveAlbumFromFavourites);
        group.MapGet("/{userId}/favorites/artists", UserEndpoints.GetLikedArtists);
        group.MapGet("/{userId}/favorites/songs", UserEndpoints.GetLikedSongs);
        group.MapGet("/{userId}/favorites/playlists", UserEndpoints.GetLikedPlaylists);
        group.MapGet("/{userId}/favorites/albums", UserEndpoints.GetLikedAlbums);
        group.MapGet("/{userId}/usersongs", UserEndpoints.GetUserSongs);
        group.MapGet("/{artistId}/artistsongs", UserEndpoints.GetArtistSongs);
    }
}