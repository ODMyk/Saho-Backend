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
using Asp.Versioning;
using Asp.Versioning.Conventions;
using SahoBackend.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;

var key = Encoding.UTF8.GetBytes(Configuration.SahoConfig.JwtSecret);
var builder = WebApplication.CreateBuilder(args);

// Set-up the CWD
string rootDirectory = FileUtils.GetRootFolder(Directory.GetCurrentDirectory(), "SahoBackend.sln");
Directory.SetCurrentDirectory(rootDirectory);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VV";
});

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigurationsOptions>();

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

// builder.Services.AddAntiforgery();

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

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/v1.0/swagger.json", "Version 1.0");
});

app.UseAuthentication();
app.UseAuthorization();

// Middlewares here
app.UseMiddleware<StrongerAuth>();
app.UseMiddleware<InjectIsAdmin>();
app.UseMiddleware<InjectIsArtist>();

app.UseCors("Restricted");
// app.UseAntiforgery();

var versionSet = app.NewApiVersionSet().HasApiVersion(1.0).Build();

// Endpoints binding
app.AddAlbumEndpoints();
app.AddAuthEndpoints();
app.AddFavoritesEndpoints();
app.AddPlaylistEndpoints();
app.AddSongEndpoints();
app.AddStorageEndpoints();
app.AddUserEndpoints();

app.Run();
