using Domain.DTOs;
using FluentValidation;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SahoBackend.Mapping.Interfaces;
using SahoBackend.Models;
using System.Net;

namespace SahoBackend.Endpoints;

public class SongEndpoints
{
    public static async Task<IResult> GetSongById(HttpContext context, int id, PostgreDbContext db, ISongMapper mapper)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var song = await db.Songs.AsNoTracking().Include(s => s.Album).Where(s => s.Id == id).FirstOrDefaultAsync();
        if (song is null || (song.Album.IsPrivate && song.ArtistId != user.Id && !(bool)context.Items["IsUserAdmin"]!))
        {
            return Results.NotFound();
        }

        await db.Entry(song).Reference(s => s.Artist).LoadAsync();

        return Results.Ok(mapper.Map(song));
    }

    public static async Task<IResult> GetAllSongs(HttpContext context, PostgreDbContext db, ISongMapper mapper)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var songs = db.Songs.AsNoTracking().Include(s => s.Album).AsEnumerable();

        if (!(bool)context.Items["IsUserAdmin"]!)
        {
            songs = songs.Where(s => !s.Album.IsPrivate || s.ArtistId == user.Id);
        }

        ICollection<SongDto> songsDtos = [];
        foreach (var song in songs)
        {
            await db.Entry(song).Reference(s => s.Artist).LoadAsync();
            var isLiked = db.Database.SqlQuery<int>($"SELECT x.\"UserId\" FROM public.\"Favorite songs\" x WHERE x.\"UserId\" = {user.Id} AND x.\"SongId\" = {song.Id} LIMIT 1").AsEnumerable().Any();
            songsDtos.Add(mapper.Map(song, isLiked)!);
        }
        return Results.Ok(songsDtos);
    }

    public static async Task<IResult> PostSong(HttpContext context, [FromBody] Song song, PostgreDbContext db, IValidator<Song> validator)
    {
        var validationResult = validator.Validate(song);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        var user = (context.Items["User"] as UserEntity)!;

        var songEntity = new SongEntity { Title = song.Title, ArtistId = user.Id, TimesPlayed = 0, Artist = user, HasLyrics = song.HasLyrics, AlbumId = song.AlbumId };
        await db.AddAsync(songEntity);
        db.Entry(user).State = EntityState.Unchanged;
        await db.SaveChangesAsync();

        return Results.Ok(songEntity.Id);
    }

    public static async Task<IResult> PutSong(HttpContext context, int id, [FromBody] Song song, PostgreDbContext db, ISongMapper mapper, IValidator<Song> validator)
    {
        var user = (context.Items["User"] as UserEntity)!;
        var validationResult = validator.Validate(song);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        var songEntity = await db.Songs.FindAsync(id);
        if (songEntity is null || (songEntity.ArtistId != user.Id && !(bool)context.Items["IsUserAdmin"]!))
        {
            return Results.NotFound();
        }

        db.Update(songEntity);
        songEntity.Title = song.Title;
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteSong(int id, PostgreDbContext db, HttpContext context)
    {

        var user = (context.Items["User"] as UserEntity)!;
        var song = await db.Songs.AsNoTracking().Where(s => s.Id == id).FirstOrDefaultAsync();
        if (song is null || (song.ArtistId != user.Id && !(bool)context.Items["IsUserAdmin"]!))
        {
            return Results.NotFound();
        }

        db.Remove(song);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}