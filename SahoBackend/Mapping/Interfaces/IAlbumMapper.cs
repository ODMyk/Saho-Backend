using SahoBackend.Models;
using Entities;
using Domain.DTOs;

namespace SahoBackend.Mapping.Interfaces;

public interface IAlbumMapper
{
    public AlbumDto? ModelToDto(Album model);
    public AlbumEntity? DtoToEntity(AlbumDto dto);
    public AlbumDto? EntityToDto(AlbumEntity entity);
}
