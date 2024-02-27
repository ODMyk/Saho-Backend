using Domain.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using SahoBackend.Models;
using Microsoft.IdentityModel.Tokens;
using Entities;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using System.Net;

namespace SahoBackend.Endpoints;

public class AuthEndpoints
{
    public static async Task<IResult> Login([FromBody] UserAuthCredentials auth, IUserService service, IValidator<UserAuthCredentials> validator)
    {
        var validationResult = validator.Validate(auth);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        var user = await service.FindByLogin(auth.Login);
        if (user is null)
        {
            return Results.NotFound();
        }

        if (!BCrypt.Net.BCrypt.Verify(auth.Password, user.Password))
        {
            return Results.Unauthorized();
        }

        var token = GenerateToken(user);
        return Results.Ok(token);
    }

    public static async Task<IResult> Register([FromBody] UserRegisterCredentials auth, IUserService service, IValidator<UserRegisterCredentials> validator)
    {
        var validationResult = validator.Validate(auth);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (await service.FindByLogin(auth.Login) is not null)
        {
            return Results.Conflict();
        }

        await service.CreateAsync(new RegisterCredentialsDTO
        {
            Email = auth.Email,
            Login = auth.Login,
            Password = auth.Password,
            Nickname = auth.Nickname
        });

        return Results.Created();
    }

    public static string GenerateToken(UserEntity user)
    {
        var roleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Title));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Login),
    }.Union(roleClaims)),
            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.SahoConfig.JwtSecret)), SecurityAlgorithms.HmacSha256Signature),
            Issuer = Configuration.SahoConfig.Host,
            Audience = Configuration.SahoConfig.Host
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}