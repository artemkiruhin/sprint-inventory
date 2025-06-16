using SprintInventory.Core.Interfaces.Services.Extension;
using SprintInventory.Core.Interfaces.Services.Mapper;
using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.Mappers;

public class InventoryItemMapper : IInventoryItemMapper
{
    private readonly IUserMapper _userMapper;
    private readonly ICategoryMapper _categoryMapper;
    private readonly IRoomMapper _roomMapper;
    private readonly IItemStatusExtensionService _extItemService;

    public InventoryItemMapper(IUserMapper userMapper, ICategoryMapper categoryMapper, IRoomMapper roomMapper, IItemStatusExtensionService extItemService)
    {
        _userMapper = userMapper;
        _categoryMapper = categoryMapper;
        _roomMapper = roomMapper;
        _extItemService = extItemService;
    }
    
    public InventoryItemDetailedDTO MapToDetailedDTO(InventoryItemEntity itemEntity)
    {
        var dto = new InventoryItemDetailedDTO(
            Id: itemEntity.Id,
            Name: itemEntity.Name,
            Description: itemEntity.Description,
            InventoryNumber: itemEntity.InventoryNumber,
            SerialNumber: itemEntity.SerialNumber,
            Status: _extItemService.GetStringName(itemEntity.Status),
            CreatedAt: itemEntity.CreatedAt,
            Creator: _userMapper.MapToShortDTO(itemEntity.Creator),
            Room: itemEntity.Room != null
                ? _roomMapper.MapToShortDTO(itemEntity.Room)
                : null,
            Category: itemEntity.Category != null
                ? _categoryMapper.MapToCategoryShortDTO(itemEntity.Category)
                : null
        );
        return dto;
    }

    public List<InventoryItemDetailedDTO> MapToDetailedDTORange(List<InventoryItemEntity> itemEntities)
    {
        var dtos = itemEntities.Select(MapToDetailedDTO).ToList();
        return dtos;
    }

    public InventoryItemShortDTO MapToShortDTO(InventoryItemEntity itemEntity)
    {
        var dto = new InventoryItemShortDTO(
            Id: itemEntity.Id,
            Name: itemEntity.Name,
            Description: itemEntity.Description,
            InventoryNumber: itemEntity.InventoryNumber,
            SerialNumber: itemEntity.SerialNumber,
            Status: _extItemService.GetStringName(itemEntity.Status),
            CreatedAt: itemEntity.CreatedAt,
            CreatorId: itemEntity.Creator.Id,
            RoomName: itemEntity.Room?.Name ?? "Нет кабинета",
            Category: itemEntity.Category != null
                ? _categoryMapper.MapToCategoryShortDTO(itemEntity.Category)
                : null
        );
        return dto;
    }

    public List<InventoryItemShortDTO> MapToShortDTORange(List<InventoryItemEntity> itemEntities)
    {
        var dtos = itemEntities.Select(MapToShortDTO).ToList();
        return dtos;
    }
}