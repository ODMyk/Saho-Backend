using SahoBackend.Mapping.Interfaces;
using SahoBackend.Models;
using Domain.Services;
// using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs;
using FluentValidation;
using System.Net;

namespace SahoBackend.Endpoints;

public class UserEndpoints
{
    public static async Task<IResult> GetUserById(
        int id,
        IUserMapper mapper,
        IUserService userService)
    {
        var user = await userService.RetrieveAsync(id);
        return user is not null ? Results.Ok(mapper.EntityToDto(user)) : Results.NotFound();
    }

    public static async Task<IResult> PutUser(
        [FromBody] User user,
        IUserMapper mapper,
        IUserService userService,
        IValidator<User> validator)
    {
        var validationResult = validator.Validate(user);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        var userDto = mapper.ModelToDto(user);
        var dto = new ExtendedUserDTO {Nickname = userDto.Nickname, ProfilePicture = userDto.ProfilePicture, Email = null!, Login=null!};
        var id = await userService.UpdateAsync(dto);

        if (id < 0)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        if (user.Id is null)
        {
            return Results.CreatedAtRoute("userById", new { Id = id });
        }

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteUserById(
        int id,
        IUserService userService)
    {
        return await userService.DeleteAsync(id) ? Results.NoContent() : Results.NotFound();
    }

    public static async Task<IResult> GetAllUsers(
        IUserService userService,
        IUserMapper mapper)
    {
        return Results.Ok((from x in await userService.GetAllAsync() select mapper.EntityToDto(x)).ToList());
    }

    public static async Task<IResult> AddSongToFavourites(
        int userId,
        [FromBody] Song song,
        IUserService userService,
        ISongService songService,
        IValidator<Song> validator)
    {
        var validationResult = validator.Validate(song);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (song.Id is null)
        {
            return Results.NotFound();
        }

        var songEntity = await songService.RetrieveAsync(song.Id.Value);
        if (!await userService.LikeSong(userId, songEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> AddArtistToFavourites(
         int userId,
         [FromBody] User artist,
         IUserService userService,
         IValidator<User> validator)
    {
        var validationResult = validator.Validate(artist);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (artist.Id is null)
        {
            return Results.NotFound();
        }

        var artistEntity = await userService.RetrieveAsync(artist.Id.Value);
        if (!await userService.LikeArtist(userId, artistEntity))
        {
            return Results.NotFound();
        }
        return Results.NoContent();
    }


    public static async Task<IResult> AddAlbumToFavourites(
        int userId,
        [FromBody] Album album,
        IUserService userService,
        IAlbumService albumService,
        IValidator<Album> validator)
    {
        var validationResult = validator.Validate(album);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (album.Id is null)
        {
            return Results.NotFound();
        }

        var albumEntity = await albumService.RetrieveAsync(album.Id.Value);
        if (!await userService.LikeAlbum(userId, albumEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> AddPlaylistToFavourites(
        int userId,
        [FromBody] Playlist playlist,
        IUserService userService,
        IPlaylistService playlistService,
        IValidator<Playlist> validator)
    {
        var validationResult = validator.Validate(playlist);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (playlist.Id is null)
        {
            return Results.NotFound();
        }

        var playlistEntity = await playlistService.RetrieveAsync(playlist.Id.Value);
        if (playlistEntity is null) {
            return Results.NotFound();
        }
        if (!await userService.LikePlaylist(userId, playlistEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> RemoveArtistFromFavourites(
        int userId,
        [FromBody] User artist,
        IUserService userService,
        IValidator<User> validator)
    {
        var validationResult = validator.Validate(artist);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (artist.Id is null)
        {
            return Results.NotFound();
        }

        var artistEntity = await userService.RetrieveAsync(artist.Id.Value);
        if (!await userService.UnlikeArtist(userId, artistEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }
    public static async Task<IResult> RemoveAlbumFromFavourites(
        int userId,
        [FromBody] Album album,
        IUserService userService,
        IAlbumService albumService,
        IValidator<Album> validator)
    {
        var validationResult = validator.Validate(album);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (album.Id is null)
        {
            return Results.NotFound();
        }

        var albumEntity = await albumService.RetrieveAsync(album.Id.Value);
        if (!await userService.UnlikeAlbum(userId, albumEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> RemovePlaylistFromFavourites(
        int userId,
        [FromBody] Playlist playlist,
        IUserService userService,
        IPlaylistService playlistService,
        IValidator<Playlist> validator)
    {
        var validationResult = validator.Validate(playlist);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (playlist.Id is null)
        {
            return Results.NotFound();
        }

        var playlistEntity = await playlistService.RetrieveAsync(playlist.Id.Value);
        if (!await userService.UnlikePlaylist(userId, playlistEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> RemoveSongFromFavourites(
        int userId,
        [FromBody] Song song,
        IUserService userService,
        ISongService songService,
        IValidator<Song> validator)
    {
        var validationResult = validator.Validate(song);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
        }

        if (song.Id is null)
        {
            return Results.NotFound();
        }

        var songEntity = await songService.RetrieveAsync(song.Id.Value);
        if (!await userService.UnlikeSong(userId, songEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> GetLikedArtists(int userId, IUserService service, IUserMapper mapper) {
        var artists = await service.GetLikedArtistsAsync(userId);
        return artists is not null ? Results.Ok((from x in artists select mapper.EntityToDto(x)).ToList()) : Results.NotFound();
    }
    
    public static async Task<IResult> GetLikedSongs(int userId, IUserService service, ISongMapper mapper) {
        var songs = await service.GetLikedSongsAsync(userId);
        return songs is not null ? Results.Ok((from x in songs select mapper.EntityToDto(x)).ToList()) : Results.NotFound();
    }
    
    public static async Task<IResult> GetLikedAlbums(int userId, IUserService service, IAlbumMapper mapper) {
        var albums = await service.GetLikedAlbumsAsync(userId);
        return albums is not null ? Results.Ok((from x in albums select mapper.EntityToDto(x)).ToList()) : Results.NotFound();
    }
    
    public static async Task<IResult> GetLikedPlaylists(int userId, IUserService service, IPlaylistMapper mapper) {
        var playlists = await service.GetLikedPlaylistsAsync(userId);
        return playlists is not null ? Results.Ok((from x in playlists select mapper.EntityToDto(x)).ToList()) : Results.NotFound();
    }

    public static async Task<IResult> GetArtistSongs(int artistId, IUserService service, ISongMapper mapper) {
        var songs = await service.GetArtistSongsAsync(artistId);
        return songs is not null ? Results.Ok((from x in songs select mapper.EntityToDto(x)).ToList()) : Results.NotFound();
    }
    
    public static async Task<IResult> GetUserSongs(int userId, IUserService service, ISongMapper mapper) {
        var songs = await service.GetUserSongsAsync(userId);
        return songs is not null ? Results.Ok((from x in songs select mapper.EntityToDto(x)).ToList()) : Results.NotFound();
    }
}
