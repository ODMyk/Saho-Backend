using Infrastructure.SQL.Database;
using Infrastructure.SQL.Entities;
using Microsoft.EntityFrameworkCore;
using SahoBackend.Mapping.Interfaces;

namespace SahoBackend.Endpoints;

public static class FavoritesEndpoints
{
    public static async Task<IResult> AddSongToFavourites(
        HttpContext context,
        int songId,
        PostgreDbContext db)
    {
        var song = await db.Songs.AsNoTracking().Where(s => s.Id == songId).FirstOrDefaultAsync();
        if (song is null)
        {
            return Results.NotFound();
        }

        var user = (context.Items["User"] as UserEntity)!;
        var e = (await db.FindAsync<UserEntity>(user.Id))!;
        await db.Entry(e).Collection(u => u.LikedSongs).LoadAsync();

        if (e.LikedSongs.All(s => s.Id != song.Id))
        {
            e.LikedSongs.Add(song);
            await db.SaveChangesAsync();
        }

        return Results.NoContent();
    }

    public static async Task<IResult> AddArtistToFavourites(
         HttpContext context,
         string artistUsername,
         PostgreDbContext db)
    {
        var artist = await db.Users.AsNoTracking().Where(u => u.Nickname == artistUsername).FirstOrDefaultAsync();
        if (artist is null)
        {
            return Results.NotFound();
        }

        var user = (context.Items["User"] as UserEntity)!;
        await db.Entry(user).Collection(u => u.LikedArtists).LoadAsync();
        if (user.LikedArtists.All(a => a.Nickname != artist.Nickname))
        {
            db.Attach(user);
            user.LikedArtists.Add(artist);
            await db.SaveChangesAsync();
        }

        return Results.NoContent();
    }

    public static async Task<IResult> AddAlbumToFavourites(
        HttpContext context,
        int albumId,
        PostgreDbContext db)
    {
        var album = await db.Albums.AsNoTracking().Where(a => a.Id == albumId).FirstOrDefaultAsync();
        if (album is null)
        {
            return Results.NotFound();
        }

        var user = (context.Items["User"] as UserEntity)!;
        await db.Entry(user).Collection(u => u.LikedAlbums).LoadAsync();
        if (user.LikedAlbums.All(a => a.Id != album.Id))
        {
            db.Attach(user);
            user.LikedAlbums.Add(album);
            await db.SaveChangesAsync();
        }

        return Results.NoContent();
    }

    public static async Task<IResult> AddPlaylistToFavourites(
        HttpContext context,
        int playlistId,
        PostgreDbContext db)
    {
        var playlist = await db.Playlists.AsNoTracking().Where(p => p.Id == playlistId).FirstOrDefaultAsync();
        if (playlist is null)
        {
            return Results.NotFound();
        }

        var user = (context.Items["User"] as UserEntity)!;
        await db.Entry(user).Collection(u => u.LikedPlaylists).LoadAsync();
        if (user.LikedPlaylists.All(p => p.Id != playlist.Id))
        {
            db.Attach(user);
            user.LikedPlaylists.Add(playlist);
            await db.SaveChangesAsync();
        }

        return Results.NoContent();
    }

    public static async Task<IResult> RemoveArtistFromFavourites(
        HttpContext context,
        string artistUsername,
        PostgreDbContext db)
    {
        var user = (context.Items["User"] as UserEntity)!;
        await db.Entry(user).Collection(u => u.LikedArtists).LoadAsync();

        var artist = user.LikedArtists.Where(a => a.Nickname == artistUsername).FirstOrDefault();
        if (artist is null)
        {
            return Results.NotFound();
        }

        db.Attach(user);
        user.LikedArtists.Remove(artist);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
    public static async Task<IResult> RemoveAlbumFromFavourites(
        HttpContext context,
        int albumId,
        PostgreDbContext db)
    {
        var user = (context.Items["User"] as UserEntity)!;
        await db.Entry(user).Collection(u => u.LikedAlbums).LoadAsync();

        var album = user.LikedAlbums.Where(a => a.Id == albumId).FirstOrDefault();
        if (album is null)
        {
            return Results.NotFound();
        }

        db.Attach(user);
        user.LikedAlbums.Remove(album);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> RemovePlaylistFromFavourites(
        HttpContext context,
        int playlistId,
        PostgreDbContext db)
    {
        var user = (context.Items["User"] as UserEntity)!;
        await db.Entry(user).Collection(u => u.LikedPlaylists).LoadAsync();

        var playlist = user.LikedPlaylists.Where(p => p.Id == playlistId).FirstOrDefault();
        if (playlist is null)
        {
            return Results.NotFound();
        }

        db.Attach(user);
        user.LikedPlaylists.Remove(playlist);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> RemoveSongFromFavourites(
        HttpContext context,
        int songId,
        PostgreDbContext db)
    {
        var user = (context.Items["User"] as UserEntity)!;

        var e = await db.FindAsync<UserEntity>(user.Id);
        await db.Entry(e!).Collection(u => u.LikedSongs).LoadAsync();
        var song = e!.LikedSongs.Where(s => s.Id == songId).FirstOrDefault();
        if (song is null)
        {
            return Results.NotFound();
        }
        e!.LikedSongs.Remove(song);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> GetLikedArtists(HttpContext context, PostgreDbContext db, IUserMapper mapper)
    {
        var artists = (await db.Users.AsNoTracking().Include(u => u.LikedArtists).Where(u => u.Nickname == context.User.Identity!.Name).FirstOrDefaultAsync())!.LikedArtists;
        return Results.Ok(from a in artists select mapper.Map(a));
    }

    public static async Task<IResult> GetLikedSongs(HttpContext context, PostgreDbContext db, ISongMapper mapper)
    {
        var user = (context.Items["User"] as UserEntity)!;
        await db.Entry(user).Collection(u => u.LikedSongs).LoadAsync();
        return Results.Ok(from s in user.LikedSongs select mapper.Map(s));
    }

    public static async Task<IResult> GetLikedAlbums(HttpContext context, PostgreDbContext db, IAlbumMapper mapper)
    {
        var user = (context.Items["User"] as UserEntity)!;
        await db.Entry(user).Collection(u => u.LikedAlbums).LoadAsync();
        return Results.Ok(from a in user.LikedAlbums select mapper.Map(a));
    }

    public static async Task<IResult> GetLikedPlaylists(HttpContext context, PostgreDbContext db, IPlaylistMapper mapper)
    {
        var playlists = (await db.Users.AsNoTracking().Include(u => u.LikedPlaylists).Where(u => u.Nickname == context.User.Identity!.Name).FirstOrDefaultAsync())!.LikedPlaylists;
        return Results.Ok(from p in playlists select mapper.Map(p));
    }
}