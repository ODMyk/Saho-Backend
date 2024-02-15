using Domain.DTOs;
using Domain.Repositories;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Repositories;
public class UserRepository : IUserRepository
{
    private readonly PostgreDbContext _context;

    public UserRepository(PostgreDbContext context)
    {
        _context = context;
    }
    public async Task<int> CreateAsync(UserDto user)
    {
        var userEntity = new UserEntity
        {
            Nickname = user.Nickame,
            RoleId = user.RoleId,
        };
        await _context.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        return userEntity.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _context.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<int> UpdateAsync(UserDto user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            Nickname = user.Nickname,
            RoleId = user.RoleId,
        };

        return await _context.Users.Where(x => x.Id == user.Id).executeUpdateAsync(s => s.SetProperty(p => p.Nickname, userEntity.Nickname).SetProperty(p => p.RoleId, user.RoleId));
    }

    public async Task<UserDto> RetreiveAsync(int id)
    {
        return await _context.AsNoTracking().Where(x => x.Id == id).Select(x => new UserDto { Id = x.Id, Nickname = x.Nickname, RoleId = x.RoleId }).FirstOrDefaultAsync();
    }

    public async Task<IList<UserDto>> GetAllAsync()
    {
        return await _context.AsNoTracking().Select(x => new UserDto { Id = x.Id, Nickname = x.Nickname, RoleId = x.RoleId }).ToListAsync();
    }
}
