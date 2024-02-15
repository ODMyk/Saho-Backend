using Domain.DTOs;
namespace Domain.Services;
public interface IPlaylistService
{
    PlaylistDto Retrieve(int id);
    IList<PlaylistDto> GetAll();
    int CreateOrUpdate(PlaylistDto playlist);
    bool Delete(int id);
}