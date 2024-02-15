using Domain.DTOs;
namespace Domain.Services;
public interface ISongService
{
    SongDto Retrieve(int id);
    IList<SongDto> GetAll();
    int CreateOrUpdate(SongDto song);
    bool Delete(int id);
}