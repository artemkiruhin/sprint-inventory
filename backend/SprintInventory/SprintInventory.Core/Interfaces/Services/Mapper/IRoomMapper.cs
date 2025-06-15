using SprintInventory.Core.Models.DTOs.Detailed;
using SprintInventory.Core.Models.DTOs.Short;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Interfaces.Services.Mapper;

public interface IRoomMapper
{
    List<RoomDetailedDTO> MapToDetailedDTORange(List<RoomEntity> roomEntities);
    RoomDetailedDTO MapToDetailedDTO(RoomEntity roomEntity);
    RoomShortDTO MapToShortDTO(RoomEntity roomEntity);
    List<RoomShortDTO> MapToShortDTORange(List<RoomEntity> roomEntities);
}