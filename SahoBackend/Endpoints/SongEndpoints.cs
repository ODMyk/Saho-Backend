using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using SahoBackend.Mapping.Interfaces;
using SahoBackend.Models;
using System.Diagnostics;

namespace SahoBackend.Endpoints;

public class SongEndpoints
{
    public static async Task<IResult> GetSongById(int id, ISongService service, ISongMapper mapper)
    {
        var entity = await service.RetrieveAsync(id);
        return entity is not null ? Results.Ok(mapper.EntityToDto(entity)) : Results.NotFound();
    }

    public static async Task<IResult> GetAllSongs(ISongService service, ISongMapper mapper)
    {
        return Results.Ok((from x in await service.GetAllAsync() select mapper.EntityToDto(x)).ToList());
    }

    public static async Task<IResult> PostSong([FromBody] Song song, ISongService service, ISongMapper mapper)
    {
        var dto = mapper.ModelToDto(song);
        return Results.CreatedAtRoute("songById", new { Id = await service.CreateOrUpdateAsync(dto) });
    }

    public static async Task<IResult> PutSong([FromBody] Song song, ISongService service, ISongMapper mapper)
    {
        var dto = mapper.ModelToDto(song);
        var id = await service.CreateOrUpdateAsync(dto);

        if (id < 0)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

        if (song.Id is not null)
        {
            return Results.NoContent();
        }
        return Results.CreatedAtRoute("songById", new { Id = id });
    }

    public static async Task<IResult> DeleteSong(int id, ISongService service)
    {
        return await service.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
    }

    public static async Task<IResult> AddToAlbum(int id, [FromBody] Album album, ISongService songService, IAlbumService albumService) {
        if (album.Id is null) {
            return Results.NotFound();
        }

         var albumEntity = await albumService.RetrieveAsync(album.Id.Value);
         return await songService.AddToAlbum(id, albumEntity) ? Results.NoContent() : Results.NotFound();
    }

    public static async Task<IResult> AddToPlaylist(int id, [FromBody] Playlist playlist, ISongService songService, IPlaylistService playlistService) {
        if (playlist.Id is null) {
            return Results.NotFound();
        }

         var playlistEntity = await playlistService.RetrieveAsync(playlist.Id.Value);
         return await songService.AddToPlaylist(id, playlistEntity) ? Results.NoContent() : Results.NotFound();
    }

    public static async Task<IResult> RemoveFromAlbum(int id, [FromBody] Album album, ISongService songService, IAlbumService albumService) {
        if (album.Id is null) {
            return Results.NotFound();
        }

        var albumEntity = await albumService.RetrieveAsync(album.Id.Value);
        return await songService.RemoveFromAlbum(id, albumEntity) ? Results.NoContent() : Results.NotFound();
    }

    public static async Task<IResult> RemoveFromPlaylist(int id, [FromBody] Playlist playlist, ISongService songService, IPlaylistService playlistService) {
        if (playlist.Id is null) {
            return Results.NotFound();
        }

        var playlistEntity = await playlistService.RetrieveAsync(playlist.Id.Value);
        return await songService.RemoveFromPlaylist(id, playlistEntity) ? Results.NoContent() : Results.NotFound();
    }
}