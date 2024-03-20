using Infrastructure.SQL.Database;
using Infrastructure.SQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace SahoBackend.Repositories;

public class UserRepository(PostgreDbContext db) : IUserRepository
{
    private readonly PostgreDbContext _db = db;

    public async Task<UserEntity> GetUser(int id)
    {
        return await _db.Users.AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
    }
}