using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class PlaylistGroup {
    public static void AddEndpoints(this WebApplication app) {
        var group = app.MapGroup("/playlists");

        group.MapGet("/", PlaylistEndpoints.GetAllPlaylists);
        group.MapGet("/{id}", PlaylistEndpoints.GetPlaylistById).WithName("playlistById");
        group.MapPost("/", PlaylistEndpoints.PostPlaylist);
        group.MapPut("/", PlaylistEndpoints.PutPlaylist);
        group.MapDelete("/{id}", PlaylistEndpoints.DeletePlaylist);
        group.MapGet("/{id}/songs", PlaylistEndpoints.GetSongs);
    }
}