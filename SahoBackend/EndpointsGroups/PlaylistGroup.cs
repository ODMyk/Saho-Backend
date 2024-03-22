using Microsoft.AspNetCore.Authorization;
using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class PlaylistGroup
{
    public static void AddPlaylistEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/playlists");

        group.RequireAuthorization(new AuthorizeAttribute { Policy = "UserPolicy" });

        group.MapGet("/", PlaylistEndpoints.GetAllPlaylists);
        group.MapGet("/{id}", PlaylistEndpoints.GetPlaylistById).WithName("playlistById");
        group.MapPost("/", PlaylistEndpoints.PostPlaylist);
        group.MapPut("/", PlaylistEndpoints.PutPlaylist);
        group.MapDelete("/{id}", PlaylistEndpoints.DeletePlaylist);
        group.MapGet("/{id}/songs", PlaylistEndpoints.GetSongs);
        group.MapPost("/{playlistId}/songs/{songId}", PlaylistEndpoints.AddSong);
        group.MapDelete("/{playlistId}/songs/{songId}", PlaylistEndpoints.RemoveSong);
    }
}