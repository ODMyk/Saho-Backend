using SahoBackend.Models;
using Entities;
using Domain.DTOs;

namespace SahoBackend.Mapping.Interfaces;

public interface ISongMapper
{
    public SongDto? ModelToDto(Song model);
    public SongEntity? DtoToEntity(SongDto dto);
    public SongDto? EntityToDto(SongEntity entity);
}
