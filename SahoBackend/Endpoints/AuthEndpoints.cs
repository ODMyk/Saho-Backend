using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using SahoBackend.Models;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.SQL.Entities;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using System.Net;
using Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;
using SahoBackend.Mapping.Interfaces;

namespace SahoBackend.Endpoints;

public class AuthEndpoints
{
    public static async Task<IResult> Login([FromBody] UserAuthCredentials auth, PostgreDbContext db, IValidator<UserAuthCredentials> validator)
    {
        var validationResult = validator.Validate(auth);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        var user = await db.Users.AsNoTracking().Where(u => u.Login == auth.Login || u.Email == auth.Login).FirstOrDefaultAsync();
        if (user is null)
        {
            return Results.NotFound();
        }

        if (!BCrypt.Net.BCrypt.Verify(auth.Password, user.Password))
        {
            return Results.Unauthorized();
        }
        await db.Entry(user).Collection(u => u.Roles).LoadAsync();

        var token = GenerateToken(user);
        return Results.Ok(token);
    }

    public static async Task<IResult> Register([FromBody] UserRegisterCredentials auth, PostgreDbContext db, IValidator<UserRegisterCredentials> validator)
    {
        var validationResult = validator.Validate(auth);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (await db.Users.AsNoTracking().Where(u => u.Login == auth.Login || u.Email == auth.Email).FirstOrDefaultAsync() is not null)
        {
            return Results.Conflict();
        }

        var user = new UserEntity { Email = auth.Email, Login = auth.Login, Password = BCrypt.Net.BCrypt.HashPassword(auth.Password), Nickname = "!" + auth.Login };
        var defaultRole = await db.Roles.AsNoTracking().Where(r => r.Id == 1).FirstOrDefaultAsync();
        await db.AddAsync(user);
        user.Roles.Add(defaultRole!);
        db.Entry(defaultRole!).State = EntityState.Unchanged;
        await db.SaveChangesAsync();

        return Results.Created();
    }

    public static IResult GetMe(HttpContext context, IUserMapper mapper)
    {
        return Results.Ok(mapper.Map((context.Items["User"] as UserEntity)!));
    }

    public static string GenerateToken(UserEntity user)
    {
        var roleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Title));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("SecurityCode", user.SecurityCode.ToString()),
    }.Union(roleClaims)),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.SahoConfig.JwtSecret)), SecurityAlgorithms.HmacSha256Signature),
            Issuer = Configuration.SahoConfig.Host,
            Audience = Configuration.SahoConfig.Host
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}