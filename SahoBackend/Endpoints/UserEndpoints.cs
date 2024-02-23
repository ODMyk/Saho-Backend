using SahoBackend.Mapping.Interfaces;
using SahoBackend.Models;
using Domain.Services;
// using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace SahoBackend.Endpoints;

public class UserEndpoints
{
    public static async Task<IResult> PostUser(
        [FromBody] User user,
        IUserMapper mapper,
        IUserService userService)
    {
        var userDto = mapper.ModelToDto(user);
        return Results.CreatedAtRoute("userById", new { Id = await userService.CreateOrUpdateAsync(userDto) });
    }

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
        IUserService userService)
    {
        var userDto = mapper.ModelToDto(user);
        var id = await userService.CreateOrUpdateAsync(userDto);

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
        int songId,
        [FromBody] User user,
        IUserService userService,
        ISongService songService)
    {
        if (user.Id is null)
        {
            return Results.NotFound();
        }

        var songEntity = await songService.RetrieveAsync(songId);
        if (!await userService.LikeSong(user.Id.Value, songEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> AddArtistToFavourites(
         int artistId,
         [FromBody] User user,
         IUserService userService)
    {
        if (user.Id is null)
        {
            return Results.NotFound();
        }

        var artistEntity = await userService.RetrieveAsync(artistId);
        if (!await userService.LikeArtist(user.Id.Value, artistEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }


    public static async Task<IResult> AddAlbumToFavourites(
        int albumId,
        [FromBody] User user,
        IUserService userService,
        IAlbumService albumService)
    {
        if (user.Id is null)
        {
            return Results.NotFound();
        }

        var albumEntity = await albumService.RetrieveAsync(albumId);
        if (!await userService.LikeAlbum(user.Id.Value, albumEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> AddPlaylistToFavourites(
        int playlistId,
        [FromBody] User user,
        IUserService userService,
        IPlaylistService playlistService)
    {
        if (user.Id is null)
        {
            return Results.NotFound();
        }

        var playlistEntity = await playlistService.RetrieveAsync(playlistId);
        if (!await userService.LikePlaylist(user.Id.Value, playlistEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> RemoveArtistFromFavourites(
        int artistId,
        [FromBody] User user,
        IUserService userService)
    {
        if (user.Id is null)
        {
            return Results.NotFound();
        }

        var artistEntity = await userService.RetrieveAsync(artistId);
        if (!await userService.UnlikeArtist(user.Id.Value, artistEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }
    public static async Task<IResult> RemoveAlbumFromFavourites(
        int albumId,
        [FromBody] User user,
        IUserService userService,
        IAlbumService albumService)
    {
        if (user.Id is null)
        {
            return Results.NotFound();
        }

        var albumEntity = await albumService.RetrieveAsync(albumId);
        if (!await userService.UnlikeAlbum(user.Id.Value, albumEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> RemovePlaylistFromFavourites(
        int playlistId,
        [FromBody] User user,
        IUserService userService,
        IPlaylistService playlistService)
    {
        if (user.Id is null)
        {
            return Results.NotFound();
        }

        var playlistEntity = await playlistService.RetrieveAsync(playlistId);
        if (!await userService.UnlikePlaylist(user.Id.Value, playlistEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }

    public static async Task<IResult> RemoveSongFromFavourites(
        int songId,
        [FromBody] User user,
        IUserService userService,
        ISongService songService)
    {
        if (user.Id is null)
        {
            return Results.NotFound();
        }

        var songEntity = await songService.RetrieveAsync(songId);
        if (!await userService.UnlikeSong(user.Id.Value, songEntity))
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Results.NoContent();
    }
}
