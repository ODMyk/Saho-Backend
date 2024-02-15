using Domain.DTOs;
namespace Domain.Services;
public interface IUserService
{
    UserDto Retrieve(int id);
    IList<UserDto> GetAll();
    int CreateOrUpdate(UserDto user);
    bool Delete(int id);
}