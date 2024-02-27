using Domain.Repositories;
using Domain.Services;
using FluentValidation;
using BLL.Services;
using Infrastructure.SQL.Repositories;
using Infrastructure.SQL.Database;
using SahoBackend.Mapping.Interfaces;
using SahoBackend.Mapping;
using Microsoft.EntityFrameworkCore;
using SahoBackend.EndpointsGroups;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var key = Encoding.UTF8.GetBytes(Configuration.SahoConfig.JwtSecret);
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddDbContextPool<PostgreDbContext>(o => o.UseNpgsql(Configuration.SahoConfig.PostgreConnectionString, npgsqlOptionsAction: s => s.EnableRetryOnFailure(maxRetryCount: 3)));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = Configuration.SahoConfig.Host,
        ValidAudience = Configuration.SahoConfig.Host,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
    .AddPolicy("UserPolicy", policy => policy.RequireRole("User"))
    .AddPolicy("ArtistPolicy", policy => policy.RequireRole("Artist"));

builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<ISongMapper, SongMapper>();
builder.Services.AddScoped<IAlbumMapper, AlbumMapper>();
builder.Services.AddScoped<IPlaylistMapper, PlaylistMapper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Middlewares here



// Endpoints binding
AuthGroup.AddEndpoints(app);
UserGroup.AddEndpoints(app);
SongGroup.AddEndpoints(app);
AlbumGroup.AddEndpoints(app);
PlaylistGroup.AddEndpoints(app);

app.Run();
