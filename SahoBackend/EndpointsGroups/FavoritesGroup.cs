using Microsoft.AspNetCore.Authorization;
using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class FavoritesGroup
{
    public static void AddFavoritesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/favorites");

        group.RequireAuthorization(new AuthorizeAttribute { Policy = "UserPolicy" });

        group.MapGet("/albums", FavoritesEndpoints.GetLikedAlbums);
        group.MapGet("/playlists", FavoritesEndpoints.GetLikedPlaylists);
        group.MapGet("/songs", FavoritesEndpoints.GetLikedSongs);
        group.MapGet("/artists", FavoritesEndpoints.GetLikedArtists);
        group.MapPost("/albums/{albumId}", FavoritesEndpoints.AddAlbumToFavourites);
        group.MapPost("/playlists/{playlistId}", FavoritesEndpoints.AddPlaylistToFavourites);
        group.MapPost("/songs/{songId}", FavoritesEndpoints.AddSongToFavourites);
        group.MapPost("/artists/{artistUsername}", FavoritesEndpoints.AddArtistToFavourites);
        group.MapDelete("/albums/{albumId}", FavoritesEndpoints.RemoveAlbumFromFavourites);
        group.MapDelete("/playlists/{playlistId}", FavoritesEndpoints.RemovePlaylistFromFavourites);
        group.MapDelete("/songs/{songId}", FavoritesEndpoints.RemoveSongFromFavourites);
        group.MapDelete("/artists/{artistUsername}", FavoritesEndpoints.RemoveArtistFromFavourites);
    }
}