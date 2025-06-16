using SprintInventory.Core.Interfaces.Services.Entity;
using SprintInventory.Core.Interfaces.Services.Extension;
using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.Mappers;

public class LogMapper : ILogMapper
{
    private readonly IInventoryItemMapper _inventoryItemMapper;
    private readonly IUserMapper _userMapper;
    private readonly IRoomMapper _roomMapper;
    private readonly IItemStatusExtensionService _statusExt;

    public LogMapper(IInventoryItemMapper inventoryItemMapper, IUserMapper userMapper, IRoomMapper roomMapper, IItemStatusExtensionService statusExt)
    {
        _inventoryItemMapper = inventoryItemMapper;
        _userMapper = userMapper;
        _roomMapper = roomMapper;
        _statusExt = statusExt;
    }
    
    public List<CreatingLogShortDTO> MapCreatingLogShortDTORange(List<InventoryItemCreatingLogEntity> logEntities)
    {
        var dtos = logEntities.Select(entity => new CreatingLogShortDTO(
            Id: entity.Id,
            ItemId: entity.ItemId,
            UserId: entity.UserId,
            ItemNumber: entity.InventoryItem.InventoryNumber ?? "Нет номера",
            Username: entity.User.Username
        )).ToList();
        return dtos;
    }

    public List<CreatingLogDetailedDTO> MapCreatingLogDetailedDTORange(List<InventoryItemCreatingLogEntity> logEntities)
    {
        var dtos = logEntities.Select(entity => new CreatingLogDetailedDTO(
            Id: entity.Id,
            ItemId: entity.ItemId,
            UserId: entity.UserId,
            User: _userMapper.MapToShortDTO(entity.User),
            InventoryItem: _inventoryItemMapper.MapToShortDTO(entity.InventoryItem)
        )).ToList();
        return dtos;
    }

    public List<StatusLogDetailedDTO> MapStatusLogDetailedDTORange(List<InventoryItemStatusLogEntity> logEntities)
    {
        var dtos = logEntities.Select(entity => new StatusLogDetailedDTO(
            Id: entity.Id,
            StatusFrom: _statusExt.GetStringName(entity.StatusFrom),
            StatusTo: _statusExt.GetStringName(entity.StatusTo),
            Timestamp: entity.Timestamp,
            User: entity.User != null
                ? _userMapper.MapToShortDTO(entity.User)
                : null
        )).ToList();
        return dtos;
    }

    public List<MovementDetailedDTO> MapMovementDetailedDTORange(List<InventoryItemMovementEntity> logEntities)
    {
        var dtos = logEntities.Select(entity => new MovementDetailedDTO(
            Id: entity.Id,
            ItemId: entity.ItemId,
            Timestamp: entity.Timestamp,
            RoomFrom: entity.RoomFrom != null
                ? _roomMapper.MapToShortDTO(entity.RoomFrom)
                : null,
            RoomTo: entity.RoomTo != null
                ? _roomMapper.MapToShortDTO(entity.RoomTo)
                : null,
            User: entity.User != null
                ? _userMapper.MapToShortDTO(entity.User)
                : null
        )).ToList();
        return dtos;
    }
}