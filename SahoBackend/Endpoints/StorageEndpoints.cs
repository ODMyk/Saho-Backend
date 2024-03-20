using System.Security.Claims;
using Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace SahoBackend.Endpoints;

public class StorageEndpoints
{
    public static async Task<IResult> GetUserProfilePicture(int id, PostgreDbContext db, HttpContext context)
    {
        var askedUser = await db.Users.AsNoTracking().Include(u => u.Roles).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (askedUser is null)
        {
            return Results.NotFound();
        }
        if (context.User.FindFirstValue("id") != id.ToString() && !(bool)context.Items["IsUserAdmin"]! && askedUser.Roles.All(r => r.Title != "Artist"))
        {
            return Results.Forbid();
        }

        string path = Path.Combine("storage", "users", "default", "avatar.png");
        if (askedUser.HasProfilePicture)
        {
            path = Path.Combine("storage", "users", askedUser.Id.ToString());
            path = Directory.EnumerateFiles(path, "avatar.*").FirstOrDefault() ?? "";
        }

        if (path == "")
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

        var mime = FileUtils.GetMimeType(Path.GetExtension(path));
        var imageBytes = await File.ReadAllBytesAsync(path);

        return Results.File(imageBytes, mime);
    }

    public static async Task<IResult> GetSongCover(int songId, HttpContext context, PostgreDbContext db)
    {
        var song = await db.Songs.AsNoTracking().Include(s => s.Artist).Where(s => s.Id == songId).FirstOrDefaultAsync();
        if (song is null)
        {
            return Results.NotFound();
        }

        if (context.User.Identity!.Name != song.Artist.Nickname && !(bool)context.Items["IsUserAdmin"]! && song.IsPrivate)
        {
            return Results.Forbid();
        }

        string path = Path.Combine("storage", "songs", "default", "cover.png");
        if (song.HasCover)
        {
            path = Path.Combine("storage", "songs", song.Id.ToString());
            path = Directory.EnumerateFiles(path, "cover.*").FirstOrDefault() ?? "";
        }

        if (path == "")
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

        var mime = FileUtils.GetMimeType(Path.GetExtension(path));
        var imageBytes = await File.ReadAllBytesAsync(path);

        return Results.File(imageBytes, mime);
    }

    // public static async Task<IResult> GetSongLyrics(int songId, HttpContext context, PostgreDbContext db)
    // {
    //     var song = await db.Songs.AsNoTracking().Include(s => s.Artist).Where(s => s.Id == songId).FirstOrDefaultAsync();
    //     if (song is null)
    //     {
    //         return Results.NotFound();
    //     }

    //     if (context.User.Identity!.Name != song.Artist.Nickname && !(bool)context.Items["IsUserAdmin"]! && song.IsPrivate)
    //     {
    //         return Results.Forbid();
    //     }

    //     string path = Path.Combine("storage", "songs", "default", "lyrics.lrc");
    //     if (song.HasLyrics)
    //     {
    //         path = Path.Combine("storage", "songs", song.Id.ToString());
    //         path = Directory.EnumerateFiles(path, "lyrics.lrc").FirstOrDefault() ?? "";
    //     }

    //     if (path == "")
    //     {
    //         return Results.StatusCode(StatusCodes.Status500InternalServerError);
    //     }

    //     var mime = FileUtils.GetMimeType(Path.GetExtension(path));
    //     var bytes = await File.ReadAllBytesAsync(path);

    //     return Results.File(bytes, mime);
    // }

    // public static async Task<IResult> GetSongFile(int songId, HttpContext context, PostgreDbContext db)
    // {
    //     var song = await db.Songs.AsNoTracking().Include(s => s.Artist).Where(s => s.Id == songId).FirstOrDefaultAsync();
    //     if (song is null)
    //     {
    //         return Results.NotFound();
    //     }

    //     if (context.User.Identity!.Name != song.Artist.Nickname && !(bool)context.Items["IsUserAdmin"]! && song.IsPrivate)
    //     {
    //         return Results.Forbid();
    //     }

    //     string path = Path.Combine("storage", "songs", song.Id.ToString());
    //     path = Directory.EnumerateFiles(path, "audio.*").FirstOrDefault() ?? "";

    //     if (path == "")
    //     {
    //         return Results.StatusCode(StatusCodes.Status500InternalServerError);
    //     }

    //     var mime = FileUtils.GetMimeType(Path.GetExtension(path));
    //     var bytes = await File.ReadAllBytesAsync(path);

    //     return Results.File(bytes, mime);
    // }

    public static async Task<IResult> GetAlbumCover(int albumId, PostgreDbContext db)
    {
        var album = await db.Albums.AsNoTracking().Where(a => a.Id == albumId).FirstOrDefaultAsync();
        if (album is null)
        {
            return Results.NotFound();
        }

        string path = Path.Combine("storage", "albums", "default", "cover.png");
        if (album.HasCover)
        {
            path = Path.Combine("storage", "songs", album.Id.ToString());
            path = Directory.EnumerateFiles(path, "cover.*").FirstOrDefault() ?? "";
        }

        if (path == "")
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

        var mime = FileUtils.GetMimeType(Path.GetExtension(path));
        var bytes = await File.ReadAllBytesAsync(path);

        return Results.File(bytes, mime);
    }

    public static async Task<IResult> GetPlaylistCover(int playlistId, HttpContext context, PostgreDbContext db)
    {
        var playlist = await db.Playlists.AsNoTracking().Include(p => p.Owner).Where(a => a.Id == playlistId).FirstOrDefaultAsync();
        if (playlist is null)
        {
            return Results.NotFound();
        }

        if (context.User.Identity!.Name != playlist.Owner.Nickname && !(bool)context.Items["IsUserAdmin"]! && playlist.IsPrivate)
        {
            return Results.Forbid();
        }

        string path = Path.Combine("storage", "playlists", "default", "cover.png");
        if (playlist.HasCover)
        {
            path = Path.Combine("storage", "playlists", playlist.Id.ToString());
            path = Directory.EnumerateFiles(path, "cover.*").FirstOrDefault() ?? "";
        }

        if (path == "")
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

        var mime = FileUtils.GetMimeType(Path.GetExtension(path));
        var bytes = await File.ReadAllBytesAsync(path);

        return Results.File(bytes, mime);
    }
}