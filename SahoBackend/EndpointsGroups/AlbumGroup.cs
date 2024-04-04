using Microsoft.AspNetCore.Authorization;
using SahoBackend.Endpoints;

namespace SahoBackend.EndpointsGroups;

public static class AlbumGroup
{
    public static void AddAlbumEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/albums");

        group.RequireAuthorization(new AuthorizeAttribute { Policy = "UserPolicy" });

        group.MapGet("/{id}", AlbumEndpoints.GetAlbumById).WithName("albumById");
        group.MapGet("/", AlbumEndpoints.GetAllAlbums);
        group.MapPost("/", AlbumEndpoints.PostAlbum).RequireAuthorization(new AuthorizeAttribute { Policy = "ArtistPolicy" });
        group.MapPut("/", AlbumEndpoints.PutAlbum);
        group.MapDelete("/{id}", AlbumEndpoints.DeleteAlbum);
        group.MapGet("/{id}/songs", AlbumEndpoints.GetSongs);
        group.MapDelete("/{albumId}/songs/{songId}", AlbumEndpoints.RemoveSong).RequireAuthorization(new AuthorizeAttribute { Policy = "ArtistPolicy" }); ;
    }
}