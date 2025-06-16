using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Interfaces.Services.Mapper;

public interface ILogMapper
{
    List<CreatingLogShortDTO> MapCreatingLogShortDTORange(List<InventoryItemCreatingLogEntity> logEntities);
    List<CreatingLogDetailedDTO> MapCreatingLogDetailedDTORange(List<InventoryItemCreatingLogEntity> logEntities);
    List<StatusLogDetailedDTO> MapStatusLogDetailedDTORange(List<InventoryItemStatusLogEntity> logEntities);
    List<MovementDetailedDTO> MapMovementDetailedDTORange(List<InventoryItemMovementEntity> logEntities);
    
}