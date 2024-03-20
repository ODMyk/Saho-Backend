using Infrastructure.SQL.Entities;

namespace SahoBackend.Repositories;

public interface IUserRepository
{
    Task<UserEntity> GetUser(int id);
}