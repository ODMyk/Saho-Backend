using SahoBackend.Models;
using Infrastructure.SQL.Entities;
using Domain.DTOs;

namespace SahoBackend.Mapping.Interfaces;

public interface IUserMapper
{
    public UserDto? Map(User user);
    public UserDto? Map(UserEntity user);
}
