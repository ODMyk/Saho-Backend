using SahoBackend.Models;
using Entities;
using Domain.DTOs;

namespace SahoBackend.Mapping.Interfaces;

public interface IUserMapper
{
    public UserDto? ModelToDto(User user);
    public UserEntity? DtoToEntity(UserDto userDto);
    public UserDto? EntityToDto(UserEntity user);
}
