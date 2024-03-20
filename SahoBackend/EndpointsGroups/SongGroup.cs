using Microsoft.AspNetCore.Authorization;
using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class SongGroup {
    public static void AddEndpoints(this WebApplication app) {
        var group = app.MapGroup("/songs");

        group.RequireAuthorization(new AuthorizeAttribute {Policy = "UserPolicy"});

        group.MapGet("/", SongEndpoints.GetAllSongs);
        group.MapGet("/{id}", SongEndpoints.GetSongById).WithName("songById");
        group.MapPost("/", SongEndpoints.PostSong);
        group.MapPut("/{id}", SongEndpoints.PutSong);
        group.MapDelete("/{id}", SongEndpoints.DeleteSong);
    }
}