using Domain.DTOs;
namespace Domain.Services;
public interface IAlbumService
{
    CountryDto Retrieve(int id);
    IList<CountryDto> GetAll();
    int CreateOrUpdate(AlbumDto album);
    bool Delete(int id);
}