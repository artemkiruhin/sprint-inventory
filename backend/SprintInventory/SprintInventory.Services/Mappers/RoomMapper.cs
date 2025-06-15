using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.Mappers;

public class RoomMapper : IRoomMapper
{
    private readonly IUserMapper _userMapper;

    public RoomMapper(IUserMapper userMapper)
    {
        _userMapper = userMapper;
    }
    
    public List<RoomDetailedDTO> MapToDetailedDTORange(List<RoomEntity> roomEntities)
    {
        var dtos = roomEntities.Select(entity => new RoomDetailedDTO(
            Id: entity.Id,
            Name: entity.Name,
            Address: entity.Address,
            CreatedAt: entity.CreatedAt,
            Creator: new UserShortDTO(
                Id: entity.CreatorId,
                Username: entity.Creator.Username,
                IsAdmin: entity.Creator.IsAdmin
            )
        )).ToList();
        return dtos;
    }

    public RoomDetailedDTO MapToDetailedDTO(RoomEntity roomEntity)
    {
        var dto = new RoomDetailedDTO(
            Id: roomEntity.Id,
            Name: roomEntity.Name,
            Address: roomEntity.Address,
            CreatedAt: roomEntity.CreatedAt,
            Creator: _userMapper.MapToShortDTO(roomEntity.Creator)
        );
        return dto;
    }

    public RoomShortDTO MapToShortDTO(RoomEntity roomEntity)
    {
        var dtos = new RoomShortDTO(
            Id: roomEntity.Id,
            Name: roomEntity.Name,
            Address: roomEntity.Address
        );
        return dtos;
    }

    public List<RoomShortDTO> MapToShortDTORange(List<RoomEntity> roomEntities)
    {
        var dtos = roomEntities.Select(entity => new RoomShortDTO(
            Id: entity.Id,
            Name: entity.Name,
            Address: entity.Address
        )).ToList();
        return dtos;
    }
}