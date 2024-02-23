using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class SongGroup {
    public static void AddEndpoints(this WebApplication app) {
        var group = app.MapGroup("/songs");

        group.MapGet("/", SongEndpoints.GetAllSongs);
        group.MapGet("/{id}", SongEndpoints.GetSongById).WithName("songById");
        group.MapPost("/", SongEndpoints.PostSong);
        group.MapPut("/", SongEndpoints.PutSong);
        group.MapDelete("/{id}", SongEndpoints.DeleteSong);
        group.MapPost("/{id}/album", SongEndpoints.AddToAlbum);
        group.MapPost("/{id}/playlist", SongEndpoints.AddToPlaylist);
        group.MapDelete("/{id}/album", SongEndpoints.RemoveFromAlbum);
        group.MapDelete("/{id}/playlist", SongEndpoints.RemoveFromPlaylist);
    }
}