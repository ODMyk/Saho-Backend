using System.Net;
using FluentValidation;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SahoBackend.Mapping.Interfaces;
using SahoBackend.Models;

namespace SahoBackend.Endpoints;

public class PlaylistEndpoints
{
    public static async Task<IResult> GetPlaylistById(int id, PostgreDbContext db, IPlaylistMapper mapper, HttpContext context)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var playlist = await db.Playlists.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
        if (playlist is null || (playlist.IsPrivate && playlist.OwnerId != user.Id && !(bool)context.Items["IsUserAdmin"]!))
        {
            return Results.NotFound();
        }
        await db.Entry(playlist).Reference(p => p.Owner).LoadAsync();

        return Results.Ok(mapper.Map(playlist));
    }

    public static IResult GetAllPlaylists(PostgreDbContext db, IPlaylistMapper mapper, HttpContext context)
    {
        var playlists = db.Playlists.AsNoTracking().Include(p => p.Owner).AsEnumerable();
        if (!(bool)context.Items["IsUserAdmin"]!)
        {
            playlists = playlists.Where(p => !p.IsPrivate || p.Owner.Nickname == context.User.Identity!.Name);
        }

        return Results.Ok(playlists.Select(mapper.Map));
    }

    public static async Task<IResult> PostPlaylist(HttpContext context, [FromBody] Playlist playlist, PostgreDbContext db, IPlaylistMapper mapper, IValidator<Playlist> validator)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var validationResult = validator.Validate(playlist);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        var playlistEntity = new PlaylistEntity { Title = playlist.Title, HasCover = playlist.HasCover, OwnerId = user.Id, IsPrivate = playlist.IsPrivate, Owner = user };
        await db.AddAsync(playlistEntity);
        await db.SaveChangesAsync();

        return Results.CreatedAtRoute("playlistById", new { playlistEntity.Id });
    }

    public static async Task<IResult> PutPlaylist(HttpContext context, int id, [FromBody] Playlist playlist, PostgreDbContext db, IPlaylistMapper mapper, IValidator<Playlist> validator)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var validationResult = validator.Validate(playlist);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        var playlistEntity = await db.Playlists.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
        if (playlistEntity is null || (playlistEntity.OwnerId != user.Id && !(bool)context.Items["IsUserAdmin"]!))
        {
            return Results.NotFound();
        }

        await db.Entry(playlistEntity).Reference(p => p.Owner).LoadAsync();
        db.Attach(playlistEntity);
        playlistEntity.Title = playlist.Title;
        playlistEntity.HasCover = playlist.HasCover;
        playlistEntity.IsPrivate = playlist.IsPrivate;
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> DeletePlaylist(int id, PostgreDbContext db, HttpContext context)
    {
        var playlist = await db.Playlists.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
        if (playlist is null || (!(bool)context.Items["IsUserAdmin"]! && playlist.OwnerId != (context.Items["User"] as UserEntity)!.Id))
        {
            return Results.NotFound();
        }
        db.Remove(playlist);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> GetSongs(HttpContext context, int id, PostgreDbContext db, ISongMapper mapper)
    {
        var playlist = await db.Playlists.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
        if (playlist is null || (playlist.IsPrivate && playlist.OwnerId != (context.Items["User"] as UserEntity)!.Id && !(bool)context.Items["IsUserAdmin"]!))
        {
            return Results.NotFound();
        }

        await db.Entry(playlist).Collection(p => p.Songs).LoadAsync();
        foreach (var song in playlist.Songs)
        {
            await db.Entry(song).Reference(s => s.Artist).LoadAsync();
        }

        return Results.Ok(playlist.Songs.Select(x => mapper.Map(x)));
    }

    public static async Task<IResult> AddSong(int playlistId, int songId, HttpContext context, PostgreDbContext db)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var playlist = await db.Playlists.AsNoTracking().Where(p => p.Id == playlistId).FirstOrDefaultAsync();
        if (playlist is null)
        {
            return Results.NotFound();
        }

        if (playlist.OwnerId != user.Id)
        {
            return Results.Forbid();
        }

        var song = await db.Songs.AsNoTracking().Where(s => s.Id == songId).FirstOrDefaultAsync();
        if (song is null)
        {
            return Results.NotFound();
        }

        await db.Entry(song).Collection(s => s.Playlists).LoadAsync();
        if (song.Playlists.All(p => p.Id != playlistId))
        {
            db.Attach(song);
            song.Playlists.Add(playlist);
            await db.SaveChangesAsync();
        }

        return Results.NoContent();
    }

    public static async Task<IResult> RemoveSong(int playlistId, int songId, HttpContext context, PostgreDbContext db)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var playlist = await db.Playlists.AsNoTracking().Where(a => a.Id == playlistId).FirstOrDefaultAsync();
        if (playlist is null)
        {
            return Results.NotFound();
        }

        if (playlist.OwnerId != user.Id)
        {
            return Results.Forbid();
        }

        await db.Entry(playlist).Collection(a => a.Songs).LoadAsync();
        var song = playlist.Songs.Where(s => s.Id == songId).FirstOrDefault();
        if (song is null)
        {
            return Results.NotFound();
        }

        db.Attach(playlist);
        playlist.Songs.Remove(song);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}