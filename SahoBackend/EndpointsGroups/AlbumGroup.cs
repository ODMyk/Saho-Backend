using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class AlbumGroup {
    public static void AddEndpoints(this WebApplication app) {
        var group = app.MapGroup("/albums");

        group.MapGet("/", AlbumEndpoints.GetAllAlbums);
        group.MapGet("/{id}", AlbumEndpoints.GetAlbumById).WithName("albumById");
        group.MapPost("/", AlbumEndpoints.PostAlbum);
        group.MapPut("/", AlbumEndpoints.PutAlbum);
        group.MapDelete("/{id}", AlbumEndpoints.DeleteAlbum);
    }
}