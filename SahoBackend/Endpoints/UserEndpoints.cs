using SahoBackend.Mapping.Interfaces;
using SahoBackend.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;
using Infrastructure.SQL.Entities;
using Domain.DTOs;

namespace SahoBackend.Endpoints;

public class UserEndpoints
{
    public static async Task<IResult> GetUserById(
        int id,
        IUserMapper mapper,
        PostgreDbContext db,
        HttpContext context)
    {
        var user = await db.Users.AsNoTracking().Include(u => u.Roles).Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null || (user.Roles.All(r => r.Title != "Artist") && id.ToString() != context.User.FindFirst("id")!.Value))
        {
            return Results.NotFound();
        }
        return Results.Ok(mapper.Map(user));
    }

    public static IResult GetExtendedInfo(
        HttpContext context,
        PostgreDbContext db)
    {
        var userEntity = (context.Items["User"] as UserEntity)!;
        return Results.Ok(
            new ExtendedUserDTO
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                Login = userEntity.Login,
                Nickname = userEntity.Nickname
            }
        );
    }

    public static async Task<IResult> UpdateNickname(
        [FromBody] User user,
        PostgreDbContext db,
        HttpContext context,
        IValidator<User> validator)
    {
        var validationResult = validator.Validate(user);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        var userEntity = (context.Items["User"] as UserEntity)!;
        if (user.Nickname != userEntity.Nickname)
        {
            await db.Entry(userEntity).Collection(u => u.Roles).LoadAsync();

            // If user is artist and the nickname he wants to set is already used by another artist
            if ((bool)context.Items["IsUserArtist"]!
                && db.Users.Include(u => u.Roles)
                    .Where(u => u.Roles.Any(r => r.Title == "Artist"))
                    .AsNoTracking()
                    .Any(u => u.Nickname == user.Nickname))
            {
                return Results.Conflict();
            }

            userEntity = (await db.FindAsync<UserEntity>(userEntity.Id))!;
            db.Attach(userEntity);
            userEntity.Nickname = user.Nickname;
            await db.SaveChangesAsync();
        }

        return Results.NoContent();
    }

    public static async Task<IResult> UpdateProfilePicture(HttpContext context, PostgreDbContext db, IFormFile picture)
    {
        var user = (context.Items["User"] as UserEntity)!;
        if (!user.HasProfilePicture)
        {
            db.Attach(user);
            user.HasProfilePicture = true;
            await db.SaveChangesAsync();
        }

        string path = Path.Combine("storage", "users", user.Id.ToString());
        var toDelete = Directory.EnumerateFiles(path, "avatar.*");
        var extension = picture.FileName.Split(".").Last();
        path = Path.Combine(path, $"avatar.{extension}");
        foreach (var file in toDelete)
        {
            if (!file.EndsWith(extension))
            {
                File.Delete(file);
            }
        }

        using (var file = File.OpenWrite(path))
        {
            await picture.CopyToAsync(file);
        }

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteProfilePicture(HttpContext context, PostgreDbContext db)
    {
        var user = (context.Items["User"] as UserEntity)!;
        if (user!.HasProfilePicture)
        {
            db.Attach(user);
            user.HasProfilePicture = false;
            await db.SaveChangesAsync();
        }

        return Results.NoContent();
    }

    public static async Task<IResult> ChangePassword(HttpContext context, PostgreDbContext db, [FromBody] string password)
    {
        var user = (context.Items["User"] as UserEntity)!;
        if (BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return Results.Conflict();
        }
        db.Attach(user);

        ++user.SecurityCode;
        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteUserById(int id, PostgreDbContext db)
    {
        return await db.Users.Where(u => u.Id == id).ExecuteDeleteAsync() > 0 ? Results.NoContent() : Results.NotFound();
    }

    public static IResult GetAllUsers(
        PostgreDbContext db,
        IUserMapper mapper,
        HttpContext context)
    {
        var users = db.Users.AsNoTracking().Include(u => u.Roles).AsEnumerable();
        if (!(bool)context.Items["IsUserAdmin"]!)
        {
            users = users.Where(u => u.Roles.Any(r => r.Title == "Artist"));
        }
        return Results.Ok(users.Select(mapper.Map));
    }

    public static async Task<IResult> GetSongsOfUser(HttpContext context, int artistId, PostgreDbContext db, ISongMapper mapper)
    {
        var user = (context.Items["User"] as UserEntity)!;
        ICollection<SongDto> songsDtos = [];
        if (user.Id == artistId || (bool)context.Items["IsUserAdmin"]!)
        {
            await db.Entry(user).Collection(u => u.Songs).LoadAsync();
            foreach (var song in user.Songs)
            {
                await db.Entry(song).Reference(s => s.Artist).LoadAsync();
                var isLiked = db.Database.SqlQuery<int>($"SELECT x.\"UserId\" FROM public.\"Favorite songs\" x WHERE x.\"UserId\" = {user.Id} AND x.\"SongId\" = {song.Id} LIMIT 1").AsEnumerable().Any();
                songsDtos.Add(mapper.Map(song, isLiked)!);
            }
            return Results.Ok(songsDtos);
        }

        var artist = await db.Users.AsNoTracking().Include(u => u.Roles).Where(u => u.Id == artistId).FirstOrDefaultAsync();
        if (artist is null && artist!.Roles.All(r => r.Title != "Artist"))
        {
            return Results.NotFound();
        }

        await db.Entry(artist).Collection(a => a.Songs).LoadAsync();
        foreach (var song in artist.Songs)
        {
            if (song.IsPrivate)
            {
                continue;
            }

            await db.Entry(song).Reference(s => s.Artist).LoadAsync();
            var isLiked = db.Database.SqlQuery<int>($"SELECT x.\"UserId\" FROM public.\"Favorite songs\" x WHERE x.\"UserId\" = {user.Id} AND x.\"SongId\" = {song.Id} LIMIT 1").AsEnumerable().Any();
            songsDtos.Add(mapper.Map(song, isLiked)!);
        }
        return Results.Ok(songsDtos);
    }

    public static async Task<IResult> MakeUserArtist(HttpContext context, PostgreDbContext db)
    {
        var userEntity = (await db.FindAsync<UserEntity>((context.Items["User"] as UserEntity)!.Id))!;
        if (!(bool)context.Items["IsUserArtist"]!)
        {
            await db.Entry(userEntity).Collection(u => u.Roles).LoadAsync();
            var role = (await db.FindAsync<RoleEntity>(2))!;
            db.Attach(userEntity);
            userEntity.Roles.Add(role);
            await db.SaveChangesAsync();

            return Results.Ok(AuthEndpoints.GenerateToken(userEntity));
        }

        return Results.NoContent();
    }
}
