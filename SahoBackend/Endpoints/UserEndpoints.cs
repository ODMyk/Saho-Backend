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
    public static IResult GetExtendedInfo(
        HttpContext context)
    {
        var userEntity = (context.Items["User"] as UserEntity)!;
        return Results.Ok(
            new ExtendedUserDTO
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                Login = userEntity.Login,
            }
        );
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

        return Results.Ok(AuthEndpoints.GenerateToken(user));
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
        var users = db.Users.AsNoTracking();
        return Results.Ok(users.Select(mapper.Map));
    }

    public static async Task<IResult> MakeUserOrganization(
        HttpContext context,
        PostgreDbContext db)
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
