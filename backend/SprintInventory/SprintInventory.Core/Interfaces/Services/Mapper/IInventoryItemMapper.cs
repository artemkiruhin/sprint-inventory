using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Interfaces.Services.Mapper;

public interface IInventoryItemMapper
{
    InventoryItemDetailedDTO MapToDetailedDTO(InventoryItemEntity itemEntity);
    List<InventoryItemDetailedDTO> MapToDetailedDTORange(List<InventoryItemEntity> itemEntities);
    InventoryItemShortDTO MapToShortDTO(InventoryItemEntity itemEntity);
    List<InventoryItemShortDTO> MapToShortDTORange(List<InventoryItemEntity> itemEntities);
}