using System.Net;
using FluentValidation;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SahoBackend.Mapping.Interfaces;
using SahoBackend.Models;

namespace SahoBackend.Endpoints;

public class AlbumEndpoints
{
    public static async Task<IResult> GetAlbumById(int id, PostgreDbContext db, IAlbumMapper mapper)
    {
        var entity = await db.Albums.AsNoTracking().Include(a => a.Artist).Where(a => a.Id == id).FirstOrDefaultAsync();
        return entity is not null ? Results.Ok(mapper.Map(entity)) : Results.NotFound();
    }

    public static IResult GetAllAlbums(PostgreDbContext db, IAlbumMapper mapper)
    {
        return Results.Ok(db.Albums.AsNoTracking().Include(a => a.Artist).Select(x => mapper.Map(x)));
    }

    public static async Task<IResult> PostAlbum(HttpContext context, [FromBody] Album album, PostgreDbContext db, IValidator<Album> validator)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var validationResult = validator.Validate(album);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }
        var albumEntity = new AlbumEntity { Title = album.Title, ArtistId = user.Id, HasCover = album.HasCover, Artist = user };
        await db.AddAsync(albumEntity);
        await db.SaveChangesAsync();

        return Results.CreatedAtRoute("albumById", new { albumEntity.Id });
    }

    public static async Task<IResult> PutAlbum(HttpContext context, int id, [FromBody] Album album, PostgreDbContext db, IValidator<Album> validator)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var validationResult = validator.Validate(album);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        var albumEntity = await db.Albums.AsNoTracking().Where(a => a.Id == id).FirstOrDefaultAsync();
        if (albumEntity is null)
        {
            return Results.NotFound();
        }

        if (user.Id != albumEntity.ArtistId && !(bool)context.Items["IsUserAdmin"]!)
        {
            return Results.Forbid();
        }

        await db.Entry(albumEntity).Reference(a => a.Artist).LoadAsync();
        db.Attach(albumEntity);
        albumEntity.Title = album.Title;
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAlbum(int id, PostgreDbContext db, HttpContext context)
    {
        var album = await db.Albums.AsNoTracking().Where(a => a.Id == id).FirstOrDefaultAsync();
        if (album is null)
        {
            return Results.NotFound();
        }

        if (!(bool)context.Items["IsUserAdmin"]! && (context.Items["User"] as UserEntity)!.Id != album.ArtistId)
        {
            return Results.Forbid();
        }
        db.Remove(album);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> GetSongs(int id, PostgreDbContext db, ISongMapper mapper)
    {
        var album = await db.Albums.AsNoTracking().Where(a => a.Id == id).FirstOrDefaultAsync();
        if (album is null)
        {
            return Results.NotFound();
        }
        await db.Entry(album).Collection(a => a.Songs).LoadAsync();
        foreach (var song in album.Songs)
        {
            await db.Entry(song).Reference(s => s.Artist).LoadAsync();
        }

        return Results.Ok(album.Songs.Select(x => mapper.Map(x)));
    }

    public static async Task<IResult> AddSong(int albumId, int songId, HttpContext context, PostgreDbContext db)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var album = await db.Albums.AsNoTracking().Where(a => a.Id == albumId).FirstOrDefaultAsync();
        if (album is null)
        {
            return Results.NotFound();
        }

        if (album.ArtistId != user.Id)
        {
            return Results.Forbid();
        }

        var song = await db.Songs.AsNoTracking().Where(s => s.Id == songId).FirstOrDefaultAsync();
        if (song is null)
        {
            return Results.NotFound();
        }

        if (song.ArtistId != user.Id)
        {
            return Results.Forbid();
        }

        await db.Entry(song).Collection(s => s.Albums).LoadAsync();
        if (song.Albums.All(a => a.Id != albumId))
        {
            db.Attach(song);
            song.Albums.Add(album);
            await db.SaveChangesAsync();
        }

        return Results.NoContent();
    }

    public static async Task<IResult> RemoveSong(int albumId, int songId, HttpContext context, PostgreDbContext db)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var album = await db.Albums.AsNoTracking().Where(a => a.Id == albumId).FirstOrDefaultAsync();
        if (album is null)
        {
            return Results.NotFound();
        }

        if (album.ArtistId != user.Id)
        {
            return Results.Forbid();
        }

        await db.Entry(album).Collection(a => a.Songs).LoadAsync();
        var song = album.Songs.Where(s => s.Id == songId).FirstOrDefault();
        if (song is null)
        {
            return Results.NotFound();
        }

        if (song.ArtistId != user.Id)
        {
            return Results.Forbid();
        }

        db.Attach(album);
        album.Songs.Remove(song);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}