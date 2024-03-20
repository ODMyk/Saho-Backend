using SahoBackend.Mapping.Interfaces;
using Domain.DTOs;
using Infrastructure.SQL.Entities;
using SahoBackend.Models;

namespace SahoBackend.Mapping;

public class UserMapper : IUserMapper
{
    public UserDto? Map(UserEntity user) {
        return user is not null ? new UserDto {
            Id = user.Id,
            Nickname = user.Nickname,
        } : null;
    }

    public UserDto? Map(User user)
    {
        return user is not null ? new UserDto {
            Nickname = user.Nickname,
        } : null;
    }
}