using FluentValidation;
using Infrastructure.SQL.Database;
using SahoBackend.Mapping.Interfaces;
using SahoBackend.Mapping;
using Microsoft.EntityFrameworkCore;
using SahoBackend.EndpointsGroups;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SahoBackend.Middlewares;
using SahoBackend.Repositories;
using Utils;

var key = Encoding.UTF8.GetBytes(Configuration.SahoConfig.JwtSecret);
var builder = WebApplication.CreateBuilder(args);

// Set-up the CWD
string rootDirectory = FileUtils.GetRootFolder(Directory.GetCurrentDirectory(), "SahoBackend.sln");
Directory.SetCurrentDirectory(rootDirectory);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddDbContextPool<PostgreDbContext>(o => o.UseNpgsql(Configuration.SahoConfig.PostgreConnectionString, npgsqlOptionsAction: s => s.EnableRetryOnFailure(maxRetryCount: 3)));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Restricted", builder =>
    {
        builder.AllowAnyHeader()
        .WithMethods("GET", "POST", "DELETE")
        .WithOrigins("localhost:5173")
        .AllowCredentials();
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Middlewares here
app.UseMiddleware<StrongerAuth>();
app.UseMiddleware<InjectIsAdmin>();

app.UseCors("Restricted");

// Endpoints binding
AuthGroup.AddEndpoints(app);
UserGroup.AddEndpoints(app);
SongGroup.AddEndpoints(app);
AlbumGroup.AddEndpoints(app);
PlaylistGroup.AddEndpoints(app);
FavoritesGroup.AddEndpoints(app);
StorageGroup.AddEndpoints(app);

app.Run();
