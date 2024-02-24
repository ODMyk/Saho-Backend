using Domain.Repositories;
using Domain.Services;
using BLL.Services;
using Infrastructure.SQL.Repositories;
using Infrastructure.SQL.Database;
using SahoBackend.Mapping.Interfaces;
using SahoBackend.Mapping;
using Microsoft.EntityFrameworkCore;
using SahoBackend.EndpointsGroups;

var builder = WebApplication.CreateBuilder(args);
var dbconnection = "Host=localhost:5432; Database=SahoDB; Username=postgres; Password=postgres";
builder.Services.AddDbContextPool<PostgreDbContext>(o => o.UseNpgsql(dbconnection, npgsqlOptionsAction: s => s.EnableRetryOnFailure(maxRetryCount: 3)));
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

UserGroup.AddEndpoints(app);
SongGroup.AddEndpoints(app);
AlbumGroup.AddEndpoints(app);
PlaylistGroup.AddEndpoints(app);

app.Run();
