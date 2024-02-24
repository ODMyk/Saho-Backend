using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using SahoBackend.Mapping.Interfaces;
using SahoBackend.Models;

namespace SahoBackend.Endpoints;

public class PlaylistEndpoints
{
    public static async Task<IResult> GetPlaylistById(int id, IPlaylistService service, IPlaylistMapper mapper)
    {
        var entity = await service.RetrieveAsync(id);
        return entity is not null ? Results.Ok(mapper.EntityToDto(entity)) : Results.NotFound();
    }

    public static async Task<IResult> GetAllPlaylists(IPlaylistService service, IPlaylistMapper mapper)
    {
        return Results.Ok((from x in await service.GetAllAsync() select mapper.EntityToDto(x)).ToList());
    }

    public static async Task<IResult> PostPlaylist([FromBody] Playlist playlist, IPlaylistService service, IPlaylistMapper mapper)
    {
        var dto = mapper.ModelToDto(playlist);
        return Results.CreatedAtRoute("playlistById", new { Id = await service.CreateOrUpdateAsync(dto) });
    }

    public static async Task<IResult> PutPlaylist([FromBody] Playlist playlist, IPlaylistService service, IPlaylistMapper mapper)
    {
        var dto = mapper.ModelToDto(playlist);
        var id = await service.CreateOrUpdateAsync(dto);

        if (id < 0)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (playlist.Id is not null)
        {
            return Results.NoContent();
        }
        return Results.CreatedAtRoute("playlistById", new { Id = id });
    }

    public static async Task<IResult> DeletePlaylist(int id, IPlaylistService service)
    {
        return await service.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
    }

    public static async Task<IResult> GetSongs(int id, IPlaylistService service, ISongMapper mapper)
    {
        var songs = await service.GetSongsAsync(id);
        return songs is not null ? Results.Ok((from x in songs select mapper.EntityToDto(x)).ToList()) : Results.NotFound();
    }
}